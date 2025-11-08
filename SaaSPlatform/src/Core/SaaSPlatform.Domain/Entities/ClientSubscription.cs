namespace SaaSPlatform.Domain.Entities;

public class ClientSubscription
{
    public int Id { get; set; }
    public required string CompanyName { get; set; }
    public required string ContactEmail { get; set; }
    public required string ContactPerson { get; set; }
    public required string BusinessType { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public SubscriptionStatus Status { get; set; } = SubscriptionStatus.Pending;
    public string? AzureResourceGroup { get; set; }
    public string? WebAppUrl { get; set; }
}

public enum SubscriptionStatus
{
    Pending,
    Approved,
    Deployed,
    Rejected
}