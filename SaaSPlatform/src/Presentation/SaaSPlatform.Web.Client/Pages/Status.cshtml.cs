using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SaaSPlatform.Application.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace SaaSPlatform.Web.Client.Pages;

public class StatusModel : PageModel
{
    private readonly ILogger<StatusModel> _logger;
    private readonly IHttpClientFactory _httpClientFactory;

    public StatusModel(ILogger<StatusModel> logger, IHttpClientFactory httpClientFactory)
    {
        _logger = logger;
        _httpClientFactory = httpClientFactory;
    }

    public List<SubscriptionResponse> SubscriptionRequests { get; set; } = new();
    public bool SearchPerformed { get; set; }

    [BindProperty(SupportsGet = true)]
    [EmailAddress]
    public string? Email { get; set; }

    public async Task OnGetAsync()
    {
        if (string.IsNullOrWhiteSpace(Email))
        {
            return;
        }

        SearchPerformed = true;

        try
        {
            var client = _httpClientFactory.CreateClient("ApiClient");
            var response = await client.GetAsync("/api/subscriptions");

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions();
                var allSubscriptions = JsonSerializer.Deserialize<List<SubscriptionResponse>>(responseContent, options);

                SubscriptionRequests = allSubscriptions?
                    .Where(s => s.ContactEmail?.Contains(Email, StringComparison.OrdinalIgnoreCase) ?? false)
                    .ToList() ?? new List<SubscriptionResponse>();
            }
            else
            {
                _logger.LogError("Failed to fetch subscription details: {StatusCode}", response.StatusCode);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching subscription details");
        }
    }

    public static string GetBadgeClass(string? status)
    {
        var baseClass = "inline-flex items-center rounded-full px-3 py-1 text-xs font-semibold ring-1 ring-inset";

        return status?.ToLowerInvariant() switch
        {
            "pending" => $"{baseClass} bg-amber-50 text-amber-700 ring-amber-200",
            "approved" => $"{baseClass} bg-emerald-50 text-emerald-700 ring-emerald-200",
            "deployed" => $"{baseClass} bg-sky-50 text-sky-700 ring-sky-200",
            "rejected" => $"{baseClass} bg-rose-50 text-rose-700 ring-rose-200",
            _ => $"{baseClass} bg-slate-100 text-slate-600 ring-slate-200"
        };
    }
}
