using SaaSPlatform.Application.Models;
using System.Text;
using System.Text.Json;

namespace SaaSPlatform.Web.Admin.Mvc.Services;

public class AdminSubscriptionService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<AdminSubscriptionService> _logger;

    public AdminSubscriptionService(IHttpClientFactory httpClientFactory, ILogger<AdminSubscriptionService> logger)
    {
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }

    public async Task<List<SubscriptionResponse>> GetAllAsync()
    {
        var client = _httpClientFactory.CreateClient("ApiClient");

        try
        {
            var response = await client.GetAsync("/api/subscriptions");
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Failed to fetch subscriptions. Status {Status}", response.StatusCode);
                return new List<SubscriptionResponse>();
            }

            var stream = await response.Content.ReadAsStreamAsync();
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            return await JsonSerializer.DeserializeAsync<List<SubscriptionResponse>>(stream, options) ?? new List<SubscriptionResponse>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching subscriptions");
            return new List<SubscriptionResponse>();
        }
    }

    public async Task<bool> UpdateStatusAsync(int id, string status)
    {
        var client = _httpClientFactory.CreateClient("ApiClient");
        var payload = JsonSerializer.Serialize(status);
        var content = new StringContent(payload, Encoding.UTF8, "application/json");

        try
        {
            var response = await client.PutAsync($"/api/subscriptions/{id}/status", content);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Failed to update subscription {Id}. Status {Status}", id, response.StatusCode);
                return false;
            }

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating subscription {Id}", id);
            return false;
        }
    }

    public async Task<bool> DeployAsync(int id)
    {
        var client = _httpClientFactory.CreateClient("ApiClient");

        try
        {
            var response = await client.PostAsync($"/api/subscriptions/{id}/deploy", null);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Failed to deploy subscription {Id}. Status {Status}", id, response.StatusCode);
                return false;
            }

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deploying subscription {Id}", id);
            return false;
        }
    }
}
