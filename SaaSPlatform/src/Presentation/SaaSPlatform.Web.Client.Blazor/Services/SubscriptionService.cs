using System.Net.Http.Json;
using SaaSPlatform.Web.Client.Blazor.Models;

namespace SaaSPlatform.Web.Client.Blazor.Services;

public class SubscriptionService
{
    private readonly HttpClient _httpClient;

    public SubscriptionService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task CreateSubscriptionAsync(CreateSubscriptionRequest request, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.PostAsJsonAsync("/api/subscriptions", request, cancellationToken);
        response.EnsureSuccessStatusCode();
    }

    public async Task<IReadOnlyList<SubscriptionResponse>> GetSubscriptionsByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        var subscriptions = await _httpClient.GetFromJsonAsync<List<SubscriptionResponse>>("/api/subscriptions", cancellationToken)
            ?? new List<SubscriptionResponse>();

        return subscriptions
            .Where(subscription => subscription.ContactEmail?.Contains(email, StringComparison.OrdinalIgnoreCase) ?? false)
            .ToList();
    }
}
