using Microsoft.AspNetCore.Mvc.RazorPages;
using SaaSPlatform.Application.Models;
using System.Text.Json;

namespace SaaSPlatform.Web.Client.Pages;

public class ConfirmationModel : PageModel
{
    private readonly ILogger<ConfirmationModel> _logger;
    private readonly IHttpClientFactory _httpClientFactory;

    public ConfirmationModel(ILogger<ConfirmationModel> logger, IHttpClientFactory httpClientFactory)
    {
        _logger = logger;
        _httpClientFactory = httpClientFactory;
    }

    public SubscriptionResponse? SubscriptionRequest { get; set; }

    public async Task OnGetAsync()
    {
        // Get the subscription ID from session
        var subscriptionId = HttpContext.Session.GetInt32("SubscriptionId");
        
        if (subscriptionId.HasValue)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("ApiClient");
                var response = await client.GetAsync($"/api/subscriptions/{subscriptionId.Value}");
                
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    SubscriptionRequest = JsonSerializer.Deserialize<SubscriptionResponse>(responseContent, options);
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
    }

    public static string GetBadgeClass(string? status)
    {
        var baseClass = "inline-flex items-center rounded-full px-3 py-1 text-xs font-semibold ring-1 ring-inset";

        return status?.ToLowerInvariant() switch
        {
            "pending approval" or "pending" => $"{baseClass} bg-amber-50 text-amber-700 ring-amber-200",
            "approved" => $"{baseClass} bg-emerald-50 text-emerald-700 ring-emerald-200",
            "deployed" => $"{baseClass} bg-sky-50 text-sky-700 ring-sky-200",
            "rejected" => $"{baseClass} bg-rose-50 text-rose-700 ring-rose-200",
            _ => $"{baseClass} bg-slate-100 text-slate-600 ring-slate-200"
        };
    }
}

public class SubscriptionResponse
{
    public int Id { get; set; }
    public string CompanyName { get; set; } = string.Empty;
    public string ContactEmail { get; set; } = string.Empty;
    public string ContactPerson { get; set; } = string.Empty;
    public string BusinessType { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; }
    public string Status { get; set; } = string.Empty;
    public string? AzureResourceGroup { get; set; }
    public string? WebAppUrl { get; set; }
}
