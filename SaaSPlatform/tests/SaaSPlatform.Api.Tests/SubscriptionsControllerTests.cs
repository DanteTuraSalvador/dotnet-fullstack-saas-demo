using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SaaSPlatform.Api.Controllers;
using SaaSPlatform.Application.DTOs;
using SaaSPlatform.Application.Interfaces;
using SaaSPlatform.Domain.Entities;
using Xunit;

namespace SaaSPlatform.Api.Tests;

/// <summary>
/// Unit tests for SubscriptionsController using mocked dependencies.
/// </summary>
public class SubscriptionsControllerTests
{
    private readonly Mock<IClientSubscriptionService> _mockService;
    private readonly SubscriptionsController _controller;

    public SubscriptionsControllerTests()
    {
        _mockService = new Mock<IClientSubscriptionService>();
        _controller = new SubscriptionsController(_mockService.Object);
    }

    [Fact]
    public async Task GetAllSubscriptions_Returns_OkResult_With_Subscriptions()
    {
        // Arrange
        var subscriptions = new List<SubscriptionResponseDto>
        {
            new()
            {
                Id = 1,
                CompanyName = "Company A",
                ContactEmail = "a@example.com",
                ContactPerson = "Person A",
                BusinessType = "Tech",
                Status = "Pending"
            },
            new()
            {
                Id = 2,
                CompanyName = "Company B",
                ContactEmail = "b@example.com",
                ContactPerson = "Person B",
                BusinessType = "Finance",
                Status = "Active"
            }
        };

        _mockService.Setup(s => s.GetAllSubscriptionsAsync())
            .ReturnsAsync(subscriptions);

        // Act
        var result = await _controller.GetAllSubscriptions();

        // Assert
        var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
        var returnedSubscriptions = okResult.Value.Should().BeAssignableTo<IEnumerable<SubscriptionResponseDto>>().Subject;
        returnedSubscriptions.Should().HaveCount(2);
    }

    [Fact]
    public async Task GetSubscription_Returns_NotFound_When_Subscription_Does_Not_Exist()
    {
        // Arrange
        _mockService.Setup(s => s.GetSubscriptionByIdAsync(999))
            .ReturnsAsync((SubscriptionResponseDto?)null);

        // Act
        var result = await _controller.GetSubscription(999);

        // Assert
        result.Result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task GetSubscription_Returns_OkResult_When_Subscription_Exists()
    {
        // Arrange
        var subscription = new SubscriptionResponseDto
        {
            Id = 1,
            CompanyName = "Test Company",
            ContactEmail = "test@example.com",
            ContactPerson = "Test Person",
            BusinessType = "Technology",
            Status = "Pending"
        };

        _mockService.Setup(s => s.GetSubscriptionByIdAsync(1))
            .ReturnsAsync(subscription);

        // Act
        var result = await _controller.GetSubscription(1);

        // Assert
        var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
        var returnedSubscription = okResult.Value.Should().BeOfType<SubscriptionResponseDto>().Subject;
        returnedSubscription.Id.Should().Be(1);
        returnedSubscription.CompanyName.Should().Be("Test Company");
    }

    [Fact]
    public async Task CreateSubscription_Returns_CreatedAtAction()
    {
        // Arrange
        var createDto = new CreateSubscriptionDto
        {
            CompanyName = "New Company",
            ContactEmail = "new@example.com",
            ContactPerson = "New Person",
            BusinessType = "Tech"
        };

        var createdSubscription = new SubscriptionResponseDto
        {
            Id = 1,
            CompanyName = createDto.CompanyName,
            ContactEmail = createDto.ContactEmail,
            ContactPerson = createDto.ContactPerson,
            BusinessType = createDto.BusinessType,
            Status = "Pending"
        };

        _mockService.Setup(s => s.CreateSubscriptionAsync(createDto))
            .ReturnsAsync(createdSubscription);

        // Act
        var result = await _controller.CreateSubscription(createDto);

        // Assert
        var createdResult = result.Result.Should().BeOfType<CreatedAtActionResult>().Subject;
        createdResult.ActionName.Should().Be(nameof(SubscriptionsController.GetSubscription));
        var returnedSubscription = createdResult.Value.Should().BeOfType<SubscriptionResponseDto>().Subject;
        returnedSubscription.CompanyName.Should().Be(createDto.CompanyName);
    }

    [Fact]
    public async Task DeleteSubscription_Returns_NoContent_When_Successful()
    {
        // Arrange
        _mockService.Setup(s => s.DeleteSubscriptionAsync(1))
            .ReturnsAsync(true);

        // Act
        var result = await _controller.DeleteSubscription(1);

        // Assert
        result.Should().BeOfType<NoContentResult>();
    }

    [Fact]
    public async Task DeleteSubscription_Returns_NotFound_When_Subscription_Does_Not_Exist()
    {
        // Arrange
        _mockService.Setup(s => s.DeleteSubscriptionAsync(999))
            .ReturnsAsync(false);

        // Act
        var result = await _controller.DeleteSubscription(999);

        // Assert
        result.Should().BeOfType<NotFoundResult>();
    }
}
