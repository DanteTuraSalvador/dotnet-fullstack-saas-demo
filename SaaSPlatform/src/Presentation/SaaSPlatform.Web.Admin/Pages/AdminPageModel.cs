using Microsoft.AspNetCore.Mvc.RazorPages;
using SaaSPlatform.Application.Models;
using System.Text.Json;

namespace SaaSPlatform.Web.Admin.Pages;

public class AdminPageModel : PageModel
{
    protected readonly ILogger _logger;
    protected readonly IHttpClientFactory _httpClientFactory;

    public AdminPageModel(ILogger logger, IHttpClientFactory httpClientFactory)
    {
        _logger = logger;
        _httpClientFactory = httpClientFactory;
    }

    protected async Task<List<SubscriptionResponse>> GetAllSubscriptionsAsync()
    {
        try
        {
            var client = _httpClientFactory.CreateClient("ApiClient");
            var response = await client.GetAsync("/api/subscriptions");
            
            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                return JsonSerializer.Deserialize<List<SubscriptionResponse>>(responseContent, options) ?? new List<SubscriptionResponse>();
            }
            else
            {
                _logger.LogError("Failed to fetch subscriptions: {StatusCode}", response.StatusCode);
                return new List<SubscriptionResponse>();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching subscriptions");
            return new List<SubscriptionResponse>();
        }
    }

    protected async Task<SubscriptionResponse?> GetSubscriptionAsync(int id)
    {
        try
        {
            var client = _httpClientFactory.CreateClient("ApiClient");
            var response = await client.GetAsync($"/api/subscriptions/{id}");
            
            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                return JsonSerializer.Deserialize<SubscriptionResponse>(responseContent, options);
            }
            else
            {
                _logger.LogError("Failed to fetch subscription {Id}: {StatusCode}", id, response.StatusCode);
                return null;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching subscription {Id}", id);
            return null;
        }
    }

    protected async Task<bool> UpdateSubscriptionStatusAsync(int id, string status)
    {
        try
        {
            var client = _httpClientFactory.CreateClient("ApiClient");
            var json = JsonSerializer.Serialize(status);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            
            var response = await client.PutAsync($"/api/subscriptions/{id}/status", content);
            
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                _logger.LogError("Failed to update subscription {Id} status: {StatusCode}", id, response.StatusCode);
                return false;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating subscription {Id} status", id);
            return false;
        }
    }

    protected async Task<bool> DeploySubscriptionAsync(int id)
    {
        try
        {
            var client = _httpClientFactory.CreateClient("ApiClient");
            var response = await client.PostAsync($"/api/subscriptions/{id}/deploy", null);

            if (response.IsSuccessStatusCode)
            {
                return true;
            }

            _logger.LogError("Failed to deploy subscription {Id}: {StatusCode}", id, response.StatusCode);
            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deploying subscription {Id}", id);
            return false;
        }
    }
}

