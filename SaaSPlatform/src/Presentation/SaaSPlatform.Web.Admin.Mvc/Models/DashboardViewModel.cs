using SaaSPlatform.Application.Models;

namespace SaaSPlatform.Web.Admin.Mvc.Models;

public class DashboardViewModel
{
    public IReadOnlyList<SubscriptionResponse> RecentSubscriptions { get; set; } = Array.Empty<SubscriptionResponse>();

    public int PendingCount { get; set; }
    public int ApprovedCount { get; set; }
    public int DeployedCount { get; set; }
    public int RejectedCount { get; set; }
}
