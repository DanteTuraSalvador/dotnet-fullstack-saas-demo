using Microsoft.AspNetCore.Mvc;
using SaaSPlatform.Web.Admin.Mvc.Models;
using SaaSPlatform.Web.Admin.Mvc.Services;

namespace SaaSPlatform.Web.Admin.Mvc.Controllers;

public class DashboardController : Controller
{
    private readonly AdminSubscriptionService _subscriptions;

    public DashboardController(AdminSubscriptionService subscriptions)
    {
        _subscriptions = subscriptions;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var all = await _subscriptions.GetAllAsync();

        var model = new DashboardViewModel
        {
            PendingCount = all.Count(s => s.Status == "Pending"),
            ApprovedCount = all.Count(s => s.Status == "Approved"),
            DeployedCount = all.Count(s => s.Status == "Deployed"),
            RejectedCount = all.Count(s => s.Status == "Rejected"),
            RecentSubscriptions = all
                .OrderByDescending(s => s.CreatedDate)
                .Take(3)
                .ToList()
        };

        return View(model);
    }
}
