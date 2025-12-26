using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace SaaSPlatform.Api;

[Authorize]
public class DeploymentHub : Hub
{
    private readonly ILogger<DeploymentHub> _logger;

    public DeploymentHub(ILogger<DeploymentHub> logger)
    {
        _logger = logger;
    }

    public override async Task OnConnectedAsync()
    {
        _logger.LogInformation("Client connected: {ConnectionId}", Context.ConnectionId);
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        _logger.LogInformation("Client disconnected: {ConnectionId}", Context.ConnectionId);
        await base.OnDisconnectedAsync(exception);
    }

    public async Task JoinDeploymentGroup(int subscriptionId)
    {
        var groupName = $"deployment-{subscriptionId}";
        await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        _logger.LogInformation("Client {ConnectionId} joined group {GroupName}", Context.ConnectionId, groupName);
    }

    public async Task LeaveDeploymentGroup(int subscriptionId)
    {
        var groupName = $"deployment-{subscriptionId}";
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
        _logger.LogInformation("Client {ConnectionId} left group {GroupName}", Context.ConnectionId, groupName);
    }
}

public interface IDeploymentHubClient
{
    Task DeploymentStatusChanged(int subscriptionId, string status, string message);
    Task DeploymentProgress(int subscriptionId, int percentage, string step);
    Task DeploymentCompleted(int subscriptionId, bool success, string message);
    Task DeploymentError(int subscriptionId, string error);
}
