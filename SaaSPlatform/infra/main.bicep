// Main Bicep Template - SaaS Platform Infrastructure
// Deploys: ACR, App Service Plan, App Services (Containers), SQL Server, SQL Database, App Insights, Storage

targetScope = 'subscription'

@description('Environment name (dev, prod)')
@allowed(['dev', 'prod'])
param environment string

@description('Azure region for resources')
param location string = 'eastus'

@description('Base name for resources')
param appName string = 'saasplatform'

@description('SQL Server administrator login')
param sqlAdminLogin string = 'sqladmin'

@secure()
@description('SQL Server administrator password')
param sqlAdminPassword string

// Resource Group
resource resourceGroup 'Microsoft.Resources/resourceGroups@2021-04-01' = {
  name: 'rg-${appName}-${environment}'
  location: location
  tags: {
    environment: environment
    project: appName
  }
}

// Log Analytics Workspace
module logAnalytics 'modules/logAnalytics.bicep' = {
  scope: resourceGroup
  name: 'logAnalytics'
  params: {
    name: 'log-${appName}-${environment}'
    location: location
  }
}

// Application Insights
module appInsights 'modules/applicationInsights.bicep' = {
  scope: resourceGroup
  name: 'appInsights'
  params: {
    name: 'appi-${appName}-${environment}'
    location: location
    logAnalyticsWorkspaceId: logAnalytics.outputs.id
  }
}

// Azure Container Registry
module containerRegistry 'modules/containerRegistry.bicep' = {
  scope: resourceGroup
  name: 'containerRegistry'
  params: {
    name: 'acr${appName}${environment}'
    location: location
    sku: environment == 'prod' ? 'Standard' : 'Basic'
  }
}

// Storage Account
module storageAccount 'modules/storageAccount.bicep' = {
  scope: resourceGroup
  name: 'storageAccount'
  params: {
    name: 'st${appName}${environment}'
    location: location
    sku: environment == 'prod' ? 'Standard_GRS' : 'Standard_LRS'
  }
}

// SQL Server
module sqlServer 'modules/sqlServer.bicep' = {
  scope: resourceGroup
  name: 'sqlServer'
  params: {
    name: 'sql-${appName}-${environment}'
    location: location
    adminLogin: sqlAdminLogin
    adminPassword: sqlAdminPassword
  }
}

// SQL Database
module sqlDatabase 'modules/sqlDatabase.bicep' = {
  scope: resourceGroup
  name: 'sqlDatabase'
  params: {
    name: 'sqldb-${appName}-${environment}'
    location: location
    serverName: sqlServer.outputs.name
    sku: environment == 'prod' ? 'S1' : 'Basic'
  }
}

// App Service Plan (Linux for containers)
module appServicePlan 'modules/appServicePlan.bicep' = {
  scope: resourceGroup
  name: 'appServicePlan'
  params: {
    name: 'asp-${appName}-${environment}'
    location: location
    sku: environment == 'prod' ? 'P1v2' : 'B1'
  }
}

// App Service - API
module appServiceApi 'modules/appServiceContainer.bicep' = {
  scope: resourceGroup
  name: 'appServiceApi'
  params: {
    name: 'app-${appName}-api-${environment}'
    location: location
    appServicePlanId: appServicePlan.outputs.id
    containerRegistryName: containerRegistry.outputs.name
    containerImage: 'saasplatform-api:latest'
    appInsightsConnectionString: appInsights.outputs.connectionString
    appSettings: [
      {
        name: 'ConnectionStrings__DefaultConnection'
        value: 'Server=${sqlServer.outputs.fullyQualifiedDomainName};Database=${sqlDatabase.outputs.name};User=${sqlAdminLogin};Password=${sqlAdminPassword};TrustServerCertificate=true;'
      }
      {
        name: 'ASPNETCORE_ENVIRONMENT'
        value: environment == 'prod' ? 'Production' : 'Development'
      }
    ]
  }
}

// App Service - Client Web
module appServiceClient 'modules/appServiceContainer.bicep' = {
  scope: resourceGroup
  name: 'appServiceClient'
  params: {
    name: 'app-${appName}-client-${environment}'
    location: location
    appServicePlanId: appServicePlan.outputs.id
    containerRegistryName: containerRegistry.outputs.name
    containerImage: 'saasplatform-client:latest'
    appInsightsConnectionString: appInsights.outputs.connectionString
    appSettings: [
      {
        name: 'ApiBaseUrl'
        value: 'https://app-${appName}-api-${environment}.azurewebsites.net'
      }
      {
        name: 'ASPNETCORE_ENVIRONMENT'
        value: environment == 'prod' ? 'Production' : 'Development'
      }
    ]
  }
}

// App Service - Admin Web
module appServiceAdmin 'modules/appServiceContainer.bicep' = {
  scope: resourceGroup
  name: 'appServiceAdmin'
  params: {
    name: 'app-${appName}-admin-${environment}'
    location: location
    appServicePlanId: appServicePlan.outputs.id
    containerRegistryName: containerRegistry.outputs.name
    containerImage: 'saasplatform-admin:latest'
    appInsightsConnectionString: appInsights.outputs.connectionString
    appSettings: [
      {
        name: 'ApiBaseUrl'
        value: 'https://app-${appName}-api-${environment}.azurewebsites.net'
      }
      {
        name: 'ASPNETCORE_ENVIRONMENT'
        value: environment == 'prod' ? 'Production' : 'Development'
      }
    ]
  }
}

// Outputs
output resourceGroupName string = resourceGroup.name
output acrLoginServer string = containerRegistry.outputs.loginServer
output apiUrl string = appServiceApi.outputs.url
output clientUrl string = appServiceClient.outputs.url
output adminUrl string = appServiceAdmin.outputs.url
output appInsightsConnectionString string = appInsights.outputs.connectionString
