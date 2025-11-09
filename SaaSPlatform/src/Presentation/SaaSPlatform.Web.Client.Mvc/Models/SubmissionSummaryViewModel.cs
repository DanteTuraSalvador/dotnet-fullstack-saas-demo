namespace SaaSPlatform.Web.Client.Mvc.Models;

public class SubmissionSummaryViewModel
{
    public string? CompanyName { get; set; }
    public string? ContactEmail { get; set; }
    public string? ContactPerson { get; set; }
    public string? BusinessType { get; set; }
    public string Status { get; set; } = "Pending Approval";
    public string SuccessMessage { get; set; } = "Your Azure SaaS subscription request has been successfully submitted.";
}
