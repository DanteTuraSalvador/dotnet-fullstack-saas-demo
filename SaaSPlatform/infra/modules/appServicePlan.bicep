// App Service Plan Module (Linux for Containers)

@description('Name of the App Service Plan')
param name string

@description('Location for the resource')
param location string

@description('SKU for the App Service Plan')
@allowed(['B1', 'B2', 'B3', 'P1v2', 'P2v2', 'P3v2', 'P1v3', 'P2v3', 'P3v3'])
param sku string = 'B1'

resource appServicePlan 'Microsoft.Web/serverfarms@2023-01-01' = {
  name: name
  location: location
  kind: 'linux'
  sku: {
    name: sku
  }
  properties: {
    reserved: true // Required for Linux
  }
}

output id string = appServicePlan.id
output name string = appServicePlan.name
