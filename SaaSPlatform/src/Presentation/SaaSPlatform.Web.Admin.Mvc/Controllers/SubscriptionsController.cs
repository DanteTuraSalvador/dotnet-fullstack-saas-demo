using Microsoft.AspNetCore.Mvc;
using SaaSPlatform.Application.Models;
using SaaSPlatform.Web.Admin.Mvc.Services;

namespace SaaSPlatform.Web.Admin.Mvc.Controllers;

public class SubscriptionsController : Controller
{
    private readonly AdminSubscriptionService _subscriptions;

    public SubscriptionsController(AdminSubscriptionService subscriptions)
    {
        _subscriptions = subscriptions;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var subscriptions = await _subscriptions.GetAllAsync();
        return View(subscriptions);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateStatus(int id, string status)
    {
        if (string.IsNullOrWhiteSpace(status))
        {
            TempData["Error"] = "Invalid status value.";
            return RedirectToAction(nameof(Index));
        }

        var updated = await _subscriptions.UpdateStatusAsync(id, status);
        if (updated)
        {
            TempData["Success"] = $"Subscription #{id} updated to {status}.";
        }
        else
        {
            TempData["Error"] = $"Unable to update subscription #{id}.";
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Deploy(int id)
    {
        var deployed = await _subscriptions.DeployAsync(id);
        if (deployed)
        {
            TempData["Success"] = $"Deployment triggered for subscription #{id}.";
        }
        else
        {
            TempData["Error"] = $"Unable to deploy subscription #{id}.";
        }

        return RedirectToAction(nameof(Index));
    }
}
