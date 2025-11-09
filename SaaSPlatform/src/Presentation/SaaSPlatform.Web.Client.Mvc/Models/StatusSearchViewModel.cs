using System.ComponentModel.DataAnnotations;
using SaaSPlatform.Application.Models;

namespace SaaSPlatform.Web.Client.Mvc.Models;

public class StatusSearchViewModel
{
    [Required]
    [EmailAddress]
    [Display(Name = "Email Address")]
    public string Email { get; set; } = string.Empty;

    public List<SubscriptionResponse> Subscriptions { get; set; } = new();

    public bool SearchPerformed { get; set; }
}
