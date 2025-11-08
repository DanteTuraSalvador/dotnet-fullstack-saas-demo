using Microsoft.AspNetCore.Mvc.RazorPages;
using SaaSPlatform.Application.Models;
using System.Linq;
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
    public bool SearchPerformed { get; set; } = false;

    public async Task OnGetAsync(string? email)
    {
        if (!string.IsNullOrWhiteSpace(email))
        {
            SearchPerformed = true;
            
            try
            {
                var client = _httpClientFactory.CreateClient("ApiClient");
                var response = await client.GetAsync($"/api/subscriptions");
                
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var options = new JsonSerializerOptions();
                    var allSubscriptions = JsonSerializer.Deserialize<List<SubscriptionResponse>>(responseContent, options);
                    
                    // Filter by email (in a real implementation, this would be done on the API side)
                    SubscriptionRequests = allSubscriptions?.Where(s => s.ContactEmail?.Contains(email, StringComparison.OrdinalIgnoreCase) ?? false).ToList() ?? new List<SubscriptionResponse>();
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

