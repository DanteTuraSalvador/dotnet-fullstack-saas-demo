using Microsoft.AspNetCore.Mvc.RazorPages;
using SaaSPlatform.Application.Models;
using System.Linq;

namespace SaaSPlatform.Web.Admin.Pages;

public class DeploymentsModel : AdminPageModel
{
    public List<SubscriptionResponse> Deployments { get; set; } = new();

    public DeploymentsModel(ILogger<DeploymentsModel> logger, IHttpClientFactory httpClientFactory) : base(logger, httpClientFactory)
    {
    }

    public async Task OnGetAsync()
    {
        var allSubscriptions = await GetAllSubscriptionsAsync();
        Deployments = allSubscriptions.Where(s => s.Status == "Deployed").ToList();
    }
}