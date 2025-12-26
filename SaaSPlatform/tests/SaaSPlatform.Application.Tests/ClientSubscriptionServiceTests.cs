using FluentAssertions;
using Moq;
using SaaSPlatform.Application.DTOs;
using SaaSPlatform.Application.Interfaces;
using SaaSPlatform.Application.Services;
using SaaSPlatform.Domain.Entities;
using Xunit;

namespace SaaSPlatform.Application.Tests;

public class ClientSubscriptionServiceTests
{
    private readonly Mock<IClientSubscriptionRepository> _mockRepository;
    private readonly Mock<IAzureDeploymentService> _mockDeploymentService;
    private readonly ClientSubscriptionService _service;

    public ClientSubscriptionServiceTests()
    {
        _mockRepository = new Mock<IClientSubscriptionRepository>();
        _mockDeploymentService = new Mock<IAzureDeploymentService>();
        _service = new ClientSubscriptionService(_mockRepository.Object, _mockDeploymentService.Object);
    }

    [Fact]
    public async Task CreateSubscriptionAsync_Should_Return_Correct_Response()
    {
        // Arrange
        var subscriptionDto = new CreateSubscriptionDto
        {
            CompanyName = "Test Company",
            ContactEmail = "test@example.com",
            ContactPerson = "John Doe",
            BusinessType = "Technology"
        };

        var expectedSubscription = new ClientSubscription
        {
            Id = 1,
            CompanyName = subscriptionDto.CompanyName,
            ContactEmail = subscriptionDto.ContactEmail,
            ContactPerson = subscriptionDto.ContactPerson,
            BusinessType = subscriptionDto.BusinessType,
            CreatedDate = DateTime.UtcNow,
            SubscriptionStatus = SubscriptionStatus.Pending
        };

        _mockRepository.Setup(r => r.AddAsync(It.IsAny<ClientSubscription>()))
            .ReturnsAsync(expectedSubscription);

        // Act
        var result = await _service.CreateSubscriptionAsync(subscriptionDto);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(expectedSubscription.Id);
        result.CompanyName.Should().Be(subscriptionDto.CompanyName);
        result.ContactEmail.Should().Be(subscriptionDto.ContactEmail);
        result.ContactPerson.Should().Be(subscriptionDto.ContactPerson);
        result.BusinessType.Should().Be(subscriptionDto.BusinessType);
        result.Status.Should().Be(SubscriptionStatus.Pending.ToString());
    }

    [Fact]
    public async Task DeploySubscriptionAsync_Should_Update_Subscription_With_Deployment_Info()
    {
        // Arrange
        var subscription = new ClientSubscription
        {
            Id = 42,
            CompanyName = "Deploy Co",
            ContactEmail = "deploy@example.com",
            ContactPerson = "Jane Smith",
            BusinessType = "Finance",
            CreatedDate = DateTime.UtcNow,
            SubscriptionStatus = SubscriptionStatus.Approved
        };

        var deploymentResult = new DeploymentResult
        {
            DeploymentId = "deploy-1234",
            ResourceGroupName = "SaaSPlatform-DeployCo-RG",
            WebAppUrl = "https://deployco-saas.azurewebsites.net",
            Status = "Completed",
            Success = true
        };

        _mockRepository.Setup(r => r.GetByIdAsync(subscription.Id))
            .ReturnsAsync(subscription);

        _mockDeploymentService.Setup(s => s.DeploySubscriptionAsync(subscription))
            .ReturnsAsync(deploymentResult);

        _mockRepository.Setup(r => r.UpdateAsync(subscription))
            .ReturnsAsync(subscription);

        // Act
        var result = await _service.DeploySubscriptionAsync(subscription.Id);

        // Assert
        result.Status.Should().Be(SubscriptionStatus.Active.ToString());
        result.AzureResourceGroup.Should().Be(deploymentResult.ResourceGroupName);
        result.WebAppUrl.Should().Be(deploymentResult.WebAppUrl);

        _mockDeploymentService.Verify(s => s.DeploySubscriptionAsync(subscription), Times.Once);
        _mockRepository.Verify(r => r.UpdateAsync(subscription), Times.Once);
    }
}
