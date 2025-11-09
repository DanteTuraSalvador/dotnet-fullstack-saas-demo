using Microsoft.AspNetCore.Mvc;
using SaaSPlatform.Application.Models;
using SaaSPlatform.Web.Client.Mvc.Models;
using System.Text.Json;

namespace SaaSPlatform.Web.Client.Mvc.Controllers;

public class StatusController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<StatusController> _logger;

    public StatusController(IHttpClientFactory httpClientFactory, ILogger<StatusController> logger)
    {
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> Index(string? email)
    {
        var model = new StatusSearchViewModel
        {
            Email = email ?? string.Empty,
            SearchPerformed = !string.IsNullOrWhiteSpace(email)
        };

        if (!model.SearchPerformed)
        {
            return View(model);
        }

        if (!TryValidateModel(model))
        {
            return View(model);
        }

        var client = _httpClientFactory.CreateClient("ApiClient");

        try
        {
            var response = await client.GetAsync("/api/subscriptions");
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Failed to fetch subscriptions. Status Code: {Status}", response.StatusCode);
                ModelState.AddModelError(string.Empty, "Unable to retrieve subscriptions right now.");
                return View(model);
            }

            var content = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var subscriptions = JsonSerializer.Deserialize<List<SubscriptionResponse>>(content, options) ?? new List<SubscriptionResponse>();

            model.Subscriptions = subscriptions
                .Where(s => s.ContactEmail?.Contains(model.Email, StringComparison.OrdinalIgnoreCase) ?? false)
                .ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving subscription status");
            ModelState.AddModelError(string.Empty, "An unexpected error occurred while fetching your subscription status.");
        }

        return View(model);
    }
}
