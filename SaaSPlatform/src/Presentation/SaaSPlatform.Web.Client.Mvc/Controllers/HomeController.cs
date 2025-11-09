using Microsoft.AspNetCore.Mvc;
using SaaSPlatform.Application.DTOs;
using SaaSPlatform.Web.Client.Mvc.Models;
using System.Net.Http.Json;
using System.Text.Json;

namespace SaaSPlatform.Web.Client.Mvc.Controllers;

public class HomeController : Controller
{
    private const string SubmissionSummaryKey = "SubmissionSummary";

    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<HomeController> _logger;

    public HomeController(IHttpClientFactory httpClientFactory, ILogger<HomeController> logger)
    {
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View(new SubscriptionFormViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Index(SubscriptionFormViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var client = _httpClientFactory.CreateClient("ApiClient");

        var payload = new CreateSubscriptionDto
        {
            CompanyName = model.CompanyName,
            ContactEmail = model.ContactEmail,
            ContactPerson = model.ContactPerson,
            BusinessType = model.BusinessType
        };

        try
        {
            var response = await client.PostAsJsonAsync("/api/subscriptions", payload);
            if (response.IsSuccessStatusCode)
            {
                var summary = new SubmissionSummaryViewModel
                {
                    CompanyName = model.CompanyName,
                    ContactEmail = model.ContactEmail,
                    ContactPerson = model.ContactPerson,
                    BusinessType = model.BusinessType
                };

                TempData[SubmissionSummaryKey] = JsonSerializer.Serialize(summary);
                return RedirectToAction(nameof(Confirmation), new { email = model.ContactEmail });
            }

            _logger.LogError("Failed to create subscription. Status Code: {Status}", response.StatusCode);
            ModelState.AddModelError(string.Empty, "Unable to submit your request at the moment. Please try again later.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error submitting subscription");
            ModelState.AddModelError(string.Empty, "An unexpected error occurred. Please try again.");
        }

        return View(model);
    }

    [HttpGet]
    public IActionResult Confirmation(string? email)
    {
        SubmissionSummaryViewModel summary = new()
        {
            ContactEmail = email,
            Status = "Pending Approval"
        };

        if (TempData.TryGetValue(SubmissionSummaryKey, out var payload) && payload is string json)
        {
            var parsed = JsonSerializer.Deserialize<SubmissionSummaryViewModel>(json);
            if (parsed is not null)
            {
                summary = parsed;
            }
        }

        return View(summary);
    }

    [HttpGet]
    public IActionResult Privacy()
    {
        return View();
    }
}
