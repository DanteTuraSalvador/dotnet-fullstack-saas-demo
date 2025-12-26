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
        subscription.SubscriptionStatus.Should().Be(SubscriptionStatus.Pending);
        subscription.SubscriptionTier.Should().Be(SubscriptionTier.Basic);
        subscription.CreatedDate.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
    }

    [Theory]
    [InlineData("Pending", SubscriptionStatus.Pending)]
    [InlineData("Approved", SubscriptionStatus.Approved)]
    [InlineData("Provisioning", SubscriptionStatus.Provisioning)]
    [InlineData("Active", SubscriptionStatus.Active)]
    [InlineData("Failed", SubscriptionStatus.Failed)]
    [InlineData("Suspended", SubscriptionStatus.Suspended)]
    [InlineData("Cancelled", SubscriptionStatus.Cancelled)]
    public void SubscriptionStatus_Should_Parse_Correctly(string statusString, SubscriptionStatus expectedStatus)
    {
        // Act
        var status = Enum.Parse<SubscriptionStatus>(statusString);

        // Assert
        status.Should().Be(expectedStatus);
    }

    [Theory]
    [InlineData("Basic", SubscriptionTier.Basic)]
    [InlineData("Standard", SubscriptionTier.Standard)]
    [InlineData("Premium", SubscriptionTier.Premium)]
    [InlineData("Enterprise", SubscriptionTier.Enterprise)]
    public void SubscriptionTier_Should_Parse_Correctly(string tierString, SubscriptionTier expectedTier)
    {
        // Act
        var tier = Enum.Parse<SubscriptionTier>(tierString);

        // Assert
        tier.Should().Be(expectedTier);
    }
}
