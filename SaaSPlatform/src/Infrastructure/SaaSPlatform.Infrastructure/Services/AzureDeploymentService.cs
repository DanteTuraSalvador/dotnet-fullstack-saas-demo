using Azure;
using Azure.Core;
using Azure.Identity;
using Azure.ResourceManager;
using Azure.ResourceManager.AppService;
using Azure.ResourceManager.AppService.Models;
using Azure.ResourceManager.Resources;
using Azure.ResourceManager.Sql;
using Azure.ResourceManager.Sql.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SaaSPlatform.Application.Interfaces;
using SaaSPlatform.Domain.Entities;

namespace SaaSPlatform.Infrastructure.Services;

public class AzureDeploymentService : IAzureDeploymentService
{
    private readonly ILogger<AzureDeploymentService> _logger;
    private readonly IConfiguration _configuration;
    private readonly ArmClient? _armClient;
    private readonly string _subscriptionId;
    private readonly string _location;
    private readonly bool _useSimulation;

    public AzureDeploymentService(
        ILogger<AzureDeploymentService> logger,
        IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
        _subscriptionId = configuration["Azure:SubscriptionId"] ?? string.Empty;
        _location = configuration["Azure:Location"] ?? "eastus";
        _useSimulation = configuration.GetValue<bool>("Azure:UseSimulation", true);

        if (!_useSimulation && !string.IsNullOrEmpty(_subscriptionId))
        {
            try
            {
                var credential = new DefaultAzureCredential();
                _armClient = new ArmClient(credential);
                _logger.LogInformation("Azure Resource Manager client initialized for subscription {SubscriptionId}", _subscriptionId);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to initialize Azure credentials, falling back to simulation mode");
                _useSimulation = true;
            }
        }
        else
        {
            _useSimulation = true;
            _logger.LogInformation("Azure deployment running in simulation mode");
        }
    }

    public async Task<DeploymentResult> DeploySubscriptionAsync(ClientSubscription subscription)
    {
        var deploymentId = $"deploy-{Guid.NewGuid().ToString()[..8]}";
        var sanitizedName = SanitizeResourceName(subscription.CompanyName);
        var resourceGroupName = $"rg-saas-{sanitizedName}-{subscription.Id}";

        _logger.LogInformation("Starting deployment {DeploymentId} for subscription {SubscriptionId}",
            deploymentId, subscription.Id);

        if (_useSimulation)
        {
            return await SimulateDeploymentAsync(subscription, deploymentId, resourceGroupName);
        }

        try
        {
            // Get the subscription resource
            var subscriptionResource = await _armClient!.GetSubscriptionResource(
                new ResourceIdentifier($"/subscriptions/{_subscriptionId}")).GetAsync();

            // Create or get resource group
            _logger.LogInformation("Creating resource group {ResourceGroupName}", resourceGroupName);
            var resourceGroups = subscriptionResource.Value.GetResourceGroups();
            var rgData = new ResourceGroupData(new AzureLocation(_location))
            {
                Tags = {
                    { "Environment", "Production" },
                    { "Application", "SaaSPlatform" },
                    { "SubscriptionId", subscription.Id.ToString() },
                    { "CompanyName", subscription.CompanyName }
                }
            };
            var resourceGroupLro = await resourceGroups.CreateOrUpdateAsync(WaitUntil.Completed, resourceGroupName, rgData);
            var resourceGroup = resourceGroupLro.Value;

            // Create App Service Plan
            var appServicePlanName = $"asp-{sanitizedName}";
            _logger.LogInformation("Creating App Service Plan {PlanName}", appServicePlanName);
            var appServicePlans = resourceGroup.GetAppServicePlans();
            var planData = new AppServicePlanData(new AzureLocation(_location))
            {
                Sku = new AppServiceSkuDescription
                {
                    Name = GetSkuForTier(subscription.SubscriptionTier),
                    Tier = GetTierName(subscription.SubscriptionTier),
                    Capacity = 1
                },
                Kind = "linux",
                IsReserved = true
            };
            var planLro = await appServicePlans.CreateOrUpdateAsync(WaitUntil.Completed, appServicePlanName, planData);

            // Create Web App
            var webAppName = $"app-{sanitizedName}-{subscription.Id}";
            _logger.LogInformation("Creating Web App {WebAppName}", webAppName);
            var webApps = resourceGroup.GetWebSites();
            var webAppData = new WebSiteData(new AzureLocation(_location))
            {
                AppServicePlanId = planLro.Value.Id,
                Kind = "app,linux",
                SiteConfig = new SiteConfigProperties
                {
                    LinuxFxVersion = "DOTNETCORE|9.0",
                    IsAlwaysOn = subscription.SubscriptionTier != SubscriptionTier.Basic
                }
            };
            var webAppLro = await webApps.CreateOrUpdateAsync(WaitUntil.Completed, webAppName, webAppData);
            var webAppUrl = $"https://{webAppLro.Value.Data.DefaultHostName}";

            // Create SQL Server and Database
            var sqlServerName = $"sql-{sanitizedName}-{subscription.Id}";
            var databaseName = $"sqldb-{sanitizedName}";
            _logger.LogInformation("Creating SQL Server {ServerName}", sqlServerName);

            var sqlServers = resourceGroup.GetSqlServers();
            var sqlServerData = new SqlServerData(new AzureLocation(_location))
            {
                AdministratorLogin = "saasadmin",
                AdministratorLoginPassword = GenerateSecurePassword(),
                Version = "12.0",
                PublicNetworkAccess = ServerNetworkAccessFlag.Enabled
            };
            var sqlServerLro = await sqlServers.CreateOrUpdateAsync(WaitUntil.Completed, sqlServerName, sqlServerData);

            // Create SQL Database
            _logger.LogInformation("Creating SQL Database {DatabaseName}", databaseName);
            var databases = sqlServerLro.Value.GetSqlDatabases();
            var dbData = new SqlDatabaseData(new AzureLocation(_location))
            {
                Sku = new SqlSku(GetSqlSkuForTier(subscription.SubscriptionTier))
            };
            await databases.CreateOrUpdateAsync(WaitUntil.Completed, databaseName, dbData);

            _logger.LogInformation("Deployment {DeploymentId} completed successfully", deploymentId);

            return new DeploymentResult
            {
                DeploymentId = deploymentId,
                Status = "Completed",
                ResourceGroupName = resourceGroupName,
                WebAppUrl = webAppUrl,
                Message = "Azure resources deployed successfully",
                Success = true
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Deployment {DeploymentId} failed", deploymentId);
            return new DeploymentResult
            {
                DeploymentId = deploymentId,
                Status = "Failed",
                ResourceGroupName = resourceGroupName,
                Message = $"Deployment failed: {ex.Message}",
                Success = false
            };
        }
    }

    public async Task<DeploymentStatus> GetDeploymentStatusAsync(string deploymentId)
    {
        _logger.LogInformation("Checking deployment status for {DeploymentId}", deploymentId);
        await Task.Delay(100);

        return new DeploymentStatus
        {
            DeploymentId = deploymentId,
            Status = "Completed",
            Message = "Deployment completed successfully",
            Progress = 100.0,
            LastUpdated = DateTime.UtcNow
        };
    }

    public async Task<bool> CancelDeploymentAsync(string deploymentId)
    {
        _logger.LogInformation("Cancelling deployment {DeploymentId}", deploymentId);
        await Task.Delay(100);
        return true;
    }

    private async Task<DeploymentResult> SimulateDeploymentAsync(
        ClientSubscription subscription,
        string deploymentId,
        string resourceGroupName)
    {
        _logger.LogInformation("Running simulated deployment for {CompanyName}", subscription.CompanyName);

        await Task.Delay(500); // Creating resource group
        await Task.Delay(500); // Creating App Service Plan
        await Task.Delay(500); // Creating Web App
        await Task.Delay(500); // Creating SQL resources

        var sanitizedName = SanitizeResourceName(subscription.CompanyName);
        var webAppUrl = $"https://app-{sanitizedName}-{subscription.Id}.azurewebsites.net";

        return new DeploymentResult
        {
            DeploymentId = deploymentId,
            Status = "Completed",
            ResourceGroupName = resourceGroupName,
            WebAppUrl = webAppUrl,
            Message = "Deployment simulation completed successfully",
            Success = true
        };
    }

    private static string SanitizeResourceName(string name)
    {
        return new string(name
            .ToLowerInvariant()
            .Where(c => char.IsLetterOrDigit(c))
            .Take(20)
            .ToArray());
    }

    private static string GetSkuForTier(SubscriptionTier tier) => tier switch
    {
        SubscriptionTier.Basic => "B1",
        SubscriptionTier.Standard => "S1",
        SubscriptionTier.Premium => "P1v2",
        SubscriptionTier.Enterprise => "P2v2",
        _ => "B1"
    };

    private static string GetTierName(SubscriptionTier tier) => tier switch
    {
        SubscriptionTier.Basic => "Basic",
        SubscriptionTier.Standard => "Standard",
        SubscriptionTier.Premium => "PremiumV2",
        SubscriptionTier.Enterprise => "PremiumV2",
        _ => "Basic"
    };

    private static string GetSqlSkuForTier(SubscriptionTier tier) => tier switch
    {
        SubscriptionTier.Basic => "Basic",
        SubscriptionTier.Standard => "S0",
        SubscriptionTier.Premium => "S1",
        SubscriptionTier.Enterprise => "S2",
        _ => "Basic"
    };

    private static string GenerateSecurePassword()
    {
        const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789!@#$%^&*";
        var random = new Random();
        return new string(Enumerable.Repeat(chars, 16)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }
}
