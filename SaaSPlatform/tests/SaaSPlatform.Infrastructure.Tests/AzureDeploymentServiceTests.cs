using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using SaaSPlatform.Domain.Entities;
using SaaSPlatform.Infrastructure.Services;
using Xunit;

namespace SaaSPlatform.Infrastructure.Tests;

public class AzureDeploymentServiceTests
{
    private readonly Mock<ILogger<AzureDeploymentService>> _mockLogger;
    private readonly IConfiguration _configuration;
    private readonly AzureDeploymentService _service;

    public AzureDeploymentServiceTests()
    {
        _mockLogger = new Mock<ILogger<AzureDeploymentService>>();

        var inMemorySettings = new Dictionary<string, string?>
        {
            { "Azure:SubscriptionId", "" },
            { "Azure:Location", "eastus" },
            { "Azure:UseSimulation", "true" }
        };

        _configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings)
            .Build();

        _service = new AzureDeploymentService(_mockLogger.Object, _configuration);
    }

    [Fact]
    public async Task DeploySubscriptionAsync_Should_Return_Successful_Deployment_Result()
    {
        // Arrange
        var subscription = new ClientSubscription
        {
            Id = 1,
            CompanyName = "Test Company",
            ContactEmail = "test@example.com",
            ContactPerson = "John Doe",
            BusinessType = "Technology",
            CreatedDate = DateTime.UtcNow,
            SubscriptionStatus = SubscriptionStatus.Pending,
            SubscriptionTier = SubscriptionTier.Basic
        };

        // Act
        var result = await _service.DeploySubscriptionAsync(subscription);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Success);
        Assert.NotEmpty(result.DeploymentId);
        Assert.Equal("Completed", result.Status);
        Assert.NotEmpty(result.ResourceGroupName);
        Assert.NotEmpty(result.WebAppUrl);
    }

    [Fact]
    public async Task GetDeploymentStatusAsync_Should_Return_Deployment_Status()
    {
        // Arrange
        var deploymentId = "deploy-12345678";

        // Act
        var result = await _service.GetDeploymentStatusAsync(deploymentId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(deploymentId, result.DeploymentId);
        Assert.Equal("Completed", result.Status);
        Assert.Equal(100.0, result.Progress);
    }

    [Fact]
    public async Task CancelDeploymentAsync_Should_Return_True()
    {
        // Arrange
        var deploymentId = "deploy-12345678";

        // Act
        var result = await _service.CancelDeploymentAsync(deploymentId);

        // Assert
        Assert.True(result);
    }

    [Theory]
    [InlineData(SubscriptionTier.Basic)]
    [InlineData(SubscriptionTier.Standard)]
    [InlineData(SubscriptionTier.Premium)]
    [InlineData(SubscriptionTier.Enterprise)]
    public async Task DeploySubscriptionAsync_Should_Handle_Different_Tiers(SubscriptionTier tier)
    {
        // Arrange
        var subscription = new ClientSubscription
        {
            Id = 1,
            CompanyName = "Tier Test Company",
            ContactEmail = "tier@example.com",
            ContactPerson = "Test Person",
            BusinessType = "Technology",
            CreatedDate = DateTime.UtcNow,
            SubscriptionStatus = SubscriptionStatus.Approved,
            SubscriptionTier = tier
        };

        // Act
        var result = await _service.DeploySubscriptionAsync(subscription);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Success);
    }
}
