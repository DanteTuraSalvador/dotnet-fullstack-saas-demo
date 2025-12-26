namespace SaaSPlatform.Domain.Entities;

public class ClientSubscription
{
    public int Id { get; set; }
    public required string CompanyName { get; set; }
    public required string ContactEmail { get; set; }
    public required string ContactPerson { get; set; }
    public required string BusinessType { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public SubscriptionStatus SubscriptionStatus { get; set; } = SubscriptionStatus.Pending;
    public SubscriptionTier SubscriptionTier { get; set; } = SubscriptionTier.Basic;
    public string? AzureResourceGroup { get; set; }
    public string? DeploymentUrl { get; set; }
}

public enum SubscriptionStatus
{
    Pending,
    Approved,
    Provisioning,
    Active,
    Failed,
    Suspended,
    Cancelled
}

public enum SubscriptionTier
{
    Basic,
    Standard,
    Premium,
    Enterprise
}