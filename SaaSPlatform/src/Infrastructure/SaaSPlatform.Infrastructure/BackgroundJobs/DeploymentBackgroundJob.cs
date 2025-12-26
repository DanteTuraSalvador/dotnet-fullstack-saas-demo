using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using SaaSPlatform.Application.Interfaces;
using SaaSPlatform.Domain.Entities;

namespace SaaSPlatform.Infrastructure.BackgroundJobs;

public interface IDeploymentBackgroundJob
{
    Task ExecuteDeploymentAsync(int subscriptionId);
}

public class DeploymentBackgroundJob : IDeploymentBackgroundJob
{
    private readonly IClientSubscriptionRepository _subscriptionRepository;
    private readonly IAzureDeploymentService _deploymentService;
    private readonly ILogger<DeploymentBackgroundJob> _logger;
    private readonly IHubContext<DeploymentHub, IDeploymentHubClient> _hubContext;

    public DeploymentBackgroundJob(
        IClientSubscriptionRepository subscriptionRepository,
        IAzureDeploymentService deploymentService,
        ILogger<DeploymentBackgroundJob> logger,
        IHubContext<DeploymentHub, IDeploymentHubClient> hubContext)
    {
        _subscriptionRepository = subscriptionRepository;
        _deploymentService = deploymentService;
        _logger = logger;
        _hubContext = hubContext;
    }

    public async Task ExecuteDeploymentAsync(int subscriptionId)
    {
        _logger.LogInformation("Starting background deployment for subscription {SubscriptionId}", subscriptionId);

        var subscription = await _subscriptionRepository.GetByIdAsync(subscriptionId);
        if (subscription == null)
        {
            _logger.LogError("Subscription {SubscriptionId} not found", subscriptionId);
            return;
        }

        var groupName = $"deployment-{subscriptionId}";

        try
        {
            // Notify clients that deployment is starting
            await _hubContext.Clients.Group(groupName)
                .DeploymentStatusChanged(subscriptionId, "Starting", "Initializing deployment...");
            await _hubContext.Clients.Group(groupName)
                .DeploymentProgress(subscriptionId, 10, "Creating resource group");

            // Update subscription status
            subscription.SubscriptionStatus = SubscriptionStatus.Provisioning;
            await _subscriptionRepository.UpdateAsync(subscription);

            await _hubContext.Clients.Group(groupName)
                .DeploymentProgress(subscriptionId, 30, "Creating App Service");

            // Execute the deployment
            var result = await _deploymentService.DeploySubscriptionAsync(subscription);

            await _hubContext.Clients.Group(groupName)
                .DeploymentProgress(subscriptionId, 70, "Creating database");

            if (result.Success)
            {
                subscription.SubscriptionStatus = SubscriptionStatus.Active;
                subscription.AzureResourceGroup = result.ResourceGroupName;
                subscription.DeploymentUrl = result.WebAppUrl;
                await _subscriptionRepository.UpdateAsync(subscription);

                await _hubContext.Clients.Group(groupName)
                    .DeploymentProgress(subscriptionId, 100, "Deployment complete");
                await _hubContext.Clients.Group(groupName)
                    .DeploymentCompleted(subscriptionId, true, result.Message);

                _logger.LogInformation("Deployment completed successfully for subscription {SubscriptionId}", subscriptionId);
            }
            else
            {
                subscription.SubscriptionStatus = SubscriptionStatus.Failed;
                await _subscriptionRepository.UpdateAsync(subscription);

                await _hubContext.Clients.Group(groupName)
                    .DeploymentError(subscriptionId, result.Message);
                await _hubContext.Clients.Group(groupName)
                    .DeploymentCompleted(subscriptionId, false, result.Message);

                _logger.LogError("Deployment failed for subscription {SubscriptionId}: {Message}",
                    subscriptionId, result.Message);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error during deployment for subscription {SubscriptionId}", subscriptionId);

            subscription.SubscriptionStatus = SubscriptionStatus.Failed;
            await _subscriptionRepository.UpdateAsync(subscription);

            await _hubContext.Clients.Group(groupName)
                .DeploymentError(subscriptionId, ex.Message);
            await _hubContext.Clients.Group(groupName)
                .DeploymentCompleted(subscriptionId, false, $"Deployment failed: {ex.Message}");
        }
    }
}

// Hub interface referenced from Api project
public interface IDeploymentHubClient
{
    Task DeploymentStatusChanged(int subscriptionId, string status, string message);
    Task DeploymentProgress(int subscriptionId, int percentage, string step);
    Task DeploymentCompleted(int subscriptionId, bool success, string message);
    Task DeploymentError(int subscriptionId, string error);
}

// Placeholder hub class - actual implementation is in Api project
public class DeploymentHub : Hub<IDeploymentHubClient>
{
}
