namespace SaaSPlatform.Application.DTOs;

public class SubscriptionResponseDto
{
    public int Id { get; set; }
    public string CompanyName { get; set; } = string.Empty;
    public string ContactEmail { get; set; } = string.Empty;
    public string ContactPerson { get; set; } = string.Empty;
    public string BusinessType { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; }
    public DateTime SubmittedDate { get; set; }
    public string Status { get; set; } = string.Empty;
    public string? AzureResourceGroup { get; set; }
    public string? WebAppUrl { get; set; }
}