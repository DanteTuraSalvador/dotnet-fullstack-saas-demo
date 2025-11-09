using System.ComponentModel.DataAnnotations;

namespace SaaSPlatform.Web.Client.Blazor.Models;

public class CreateSubscriptionRequest
{
    [Required]
    public string CompanyName { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string ContactEmail { get; set; } = string.Empty;

    [Required]
    public string ContactPerson { get; set; } = string.Empty;

    [Required]
    public string BusinessType { get; set; } = string.Empty;
}

public class SubscriptionResponse
{
    public int Id { get; set; }
    public string CompanyName { get; set; } = string.Empty;
    public string ContactEmail { get; set; } = string.Empty;
    public string ContactPerson { get; set; } = string.Empty;
    public string BusinessType { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; }
    public string? AzureResourceGroup { get; set; }
    public string? WebAppUrl { get; set; }
}

public class SubmissionSummary
{
    public string CompanyName { get; set; } = string.Empty;
    public string ContactEmail { get; set; } = string.Empty;
    public string ContactPerson { get; set; } = string.Empty;
    public string BusinessType { get; set; } = string.Empty;
    public string Status { get; set; } = "Pending Approval";
}
