using SaaSPlatform.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace SaaSPlatform.Application.Interfaces;

/// <summary>
/// Service interface for Azure deployment operations
/// </summary>
public interface IAzureDeploymentService
{
    /// <summary>
    /// Deploys Azure resources for a client subscription
    /// </summary>
    /// <param name="subscription">The client subscription to deploy</param>
    /// <returns>Deployment result with status and resource information</returns>
    Task<DeploymentResult> DeploySubscriptionAsync(ClientSubscription subscription);

    /// <summary>
    /// Gets the status of a deployment
    /// </summary>
    /// <param name="deploymentId">The deployment identifier</param>
    /// <returns>Deployment status information</returns>
    Task<DeploymentStatus> GetDeploymentStatusAsync(string deploymentId);

    /// <summary>
    /// Cancels a deployment
    /// </summary>
    /// <param name="deploymentId">The deployment identifier</param>
    /// <returns>True if cancellation was successful</returns>
    Task<bool> CancelDeploymentAsync(string deploymentId);
}

/// <summary>
/// Result of an Azure deployment operation
/// </summary>
public class DeploymentResult
{
    public string DeploymentId { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string ResourceGroupName { get; set; } = string.Empty;
    public string WebAppUrl { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public bool Success { get; set; }
}

/// <summary>
/// Status information for an Azure deployment
/// </summary>
public class DeploymentStatus
{
    public string DeploymentId { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public double Progress { get; set; }
    public DateTime LastUpdated { get; set; }
}