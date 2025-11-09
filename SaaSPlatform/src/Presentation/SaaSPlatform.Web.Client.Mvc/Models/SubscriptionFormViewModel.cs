using System.ComponentModel.DataAnnotations;

namespace SaaSPlatform.Web.Client.Mvc.Models;

public class SubscriptionFormViewModel
{
    [Required]
    [Display(Name = "Company Name")]
    public string CompanyName { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [Display(Name = "Contact Email")]
    public string ContactEmail { get; set; } = string.Empty;

    [Required]
    [Display(Name = "Contact Person")]
    public string ContactPerson { get; set; } = string.Empty;

    [Required]
    [Display(Name = "Business Type")]
    public string BusinessType { get; set; } = string.Empty;
}
