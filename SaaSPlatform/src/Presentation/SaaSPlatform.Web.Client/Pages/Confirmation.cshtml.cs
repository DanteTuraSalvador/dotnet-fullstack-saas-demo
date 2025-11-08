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