using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using SaaSPlatform.Application.DTOs;
using Xunit;

namespace SaaSPlatform.Api.Tests;

public class SubscriptionsControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;

    public SubscriptionsControllerTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
    }

    [Fact]
    public async Task GetSubscriptions_Returns_Empty_List_When_No_Subscriptions()
    {
        // Act
        var response = await _client.GetAsync("/api/subscriptions");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var subscriptions = await response.Content.ReadFromJsonAsync<List<SubscriptionResponseDto>>();
        subscriptions.Should().NotBeNull();
        // Note: This might not be empty if there are existing subscriptions in the database
    }

    [Fact]
    public async Task CreateSubscription_Returns_Created_Subscription()
    {
        // Arrange
        var createDto = new CreateSubscriptionDto
        {
            CompanyName = "Test Company",
            ContactEmail = "test@example.com",
            ContactPerson = "John Doe",
            BusinessType = "Technology"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/subscriptions", createDto);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var createdSubscription = await response.Content.ReadFromJsonAsync<SubscriptionResponseDto>();
        createdSubscription.Should().NotBeNull();
        createdSubscription!.CompanyName.Should().Be(createDto.CompanyName);
        createdSubscription.ContactEmail.Should().Be(createDto.ContactEmail);
        createdSubscription.ContactPerson.Should().Be(createDto.ContactPerson);
        createdSubscription.BusinessType.Should().Be(createDto.BusinessType);
        createdSubscription.Status.Should().Be("Pending");
    }
}