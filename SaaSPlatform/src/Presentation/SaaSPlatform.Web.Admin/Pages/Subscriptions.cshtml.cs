using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SaaSPlatform.Application.Models;

namespace SaaSPlatform.Web.Admin.Pages;

public class SubscriptionsModel : AdminPageModel
{
    public List<SubscriptionResponse> Subscriptions { get; set; } = new();

    public SubscriptionsModel(ILogger<SubscriptionsModel> logger, IHttpClientFactory httpClientFactory) : base(logger, httpClientFactory)
    {
    }

    public async Task OnGetAsync()
    {
        Subscriptions = await GetAllSubscriptionsAsync();
    }

    public async Task<IActionResult> OnPostUpdateStatusAsync(int id, string status)
    {
        if (string.IsNullOrWhiteSpace(status))
        {
            ModelState.AddModelError(string.Empty, "Invalid status value.");
            Subscriptions = await GetAllSubscriptionsAsync();
            return Page();
        }

        var success = await UpdateSubscriptionStatusAsync(id, status);
        if (!success)
        {
            ModelState.AddModelError(string.Empty, "Unable to update subscription status.");
            Subscriptions = await GetAllSubscriptionsAsync();
            return Page();
        }

        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostDeployAsync(int id)
    {
        var success = await DeploySubscriptionAsync(id);
        if (!success)
        {
            ModelState.AddModelError(string.Empty, "Unable to deploy subscription.");
            Subscriptions = await GetAllSubscriptionsAsync();
            return Page();
        }

        return RedirectToPage();
    }
}
