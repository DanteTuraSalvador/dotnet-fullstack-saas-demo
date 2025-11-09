using Microsoft.AspNetCore.Mvc;
using SaaSPlatform.Application.Models;
using SaaSPlatform.Web.Admin.Mvc.Services;

namespace SaaSPlatform.Web.Admin.Mvc.Controllers;

public class DeploymentsController : Controller
{
    private readonly AdminSubscriptionService _subscriptions;

    public DeploymentsController(AdminSubscriptionService subscriptions)
    {
        _subscriptions = subscriptions;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var deployed = await _subscriptions.GetAllAsync();
        var deployments = deployed.Where(s => s.Status == "Deployed").ToList();
        return View(deployments);
    }
}
