using FluentAssertions;
using SaaSPlatform.Domain.Entities;
using Xunit;

namespace SaaSPlatform.Domain.Tests;

public class ClientSubscriptionTests
{
    [Fact]
    public void ClientSubscription_Should_Create_With_Correct_Defaults()
    {
        // Arrange
        var companyName = "Test Company";
        var contactEmail = "test@example.com";
        var contactPerson = "John Doe";
        var businessType = "Technology";

        // Act
        var subscription = new ClientSubscription
        {
            CompanyName = companyName,
            ContactEmail = contactEmail,
            ContactPerson = contactPerson,
            BusinessType = businessType
        };

        // Assert
        subscription.CompanyName.Should().Be(companyName);
        subscription.ContactEmail.Should().Be(contactEmail);
        subscription.ContactPerson.Should().Be(contactPerson);
        subscription.BusinessType.Should().Be(businessType);
        subscription.Status.Should().Be(SubscriptionStatus.Pending);
        subscription.CreatedDate.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
    }

    [Theory]
    [InlineData("Pending", SubscriptionStatus.Pending)]
    [InlineData("Approved", SubscriptionStatus.Approved)]
    [InlineData("Deployed", SubscriptionStatus.Deployed)]
    [InlineData("Rejected", SubscriptionStatus.Rejected)]
    public void SubscriptionStatus_Should_Parse_Correctly(string statusString, SubscriptionStatus expectedStatus)
    {
        // Act
        var status = Enum.Parse<SubscriptionStatus>(statusString);

        // Assert
        status.Should().Be(expectedStatus);
    }
}