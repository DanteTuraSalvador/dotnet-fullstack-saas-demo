using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SaaSPlatform.Application.Models;
using System.Linq;

namespace SaaSPlatform.Web.Admin.Pages;

public class IndexModel : AdminPageModel
{
    public List<SubscriptionResponse> Subscriptions { get; set; } = new();

    public int PendingCount { get; set; }
    public int ApprovedCount { get; set; }
    public int DeployedCount { get; set; }
    public int RejectedCount { get; set; }

    public IndexModel(ILogger<IndexModel> logger, IHttpClientFactory httpClientFactory) : base(logger, httpClientFactory)
    {
    }

    public async Task OnGetAsync()
    {
        Subscriptions = await GetAllSubscriptionsAsync();
        PopulateDashboardMetrics();
    }

    public async Task<IActionResult> OnPostUpdateStatusAsync(int id, string status)
    {
        if (string.IsNullOrWhiteSpace(status))
        {
            ModelState.AddModelError(string.Empty, "Invalid status value.");
            await ReloadDashboardAsync();
            return Page();
        }

        var success = await UpdateSubscriptionStatusAsync(id, status);
        if (!success)
        {
            ModelState.AddModelError(string.Empty, "Unable to update subscription status.");
            await ReloadDashboardAsync();
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
            await ReloadDashboardAsync();
            return Page();
        }

        return RedirectToPage();
    }

    private async Task ReloadDashboardAsync()
    {
        Subscriptions = await GetAllSubscriptionsAsync();
        PopulateDashboardMetrics();
    }

    private void PopulateDashboardMetrics()
    {
        PendingCount = Subscriptions.Count(s => s.Status == "Pending");
        ApprovedCount = Subscriptions.Count(s => s.Status == "Approved");
        DeployedCount = Subscriptions.Count(s => s.Status == "Deployed");
        RejectedCount = Subscriptions.Count(s => s.Status == "Rejected");

        Subscriptions = Subscriptions
            .OrderByDescending(s => s.CreatedDate)
            .Take(3)
            .ToList();
    }
}
