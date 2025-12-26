// Azure SQL Database Module

@description('Name of the SQL Database')
param name string

@description('Location for the resource')
param location string

@description('Name of the parent SQL Server')
param serverName string

@description('SKU for the database')
@allowed(['Basic', 'S0', 'S1', 'S2', 'S3', 'P1', 'P2', 'P4', 'P6', 'P11', 'P15'])
param sku string = 'Basic'

resource sqlServer 'Microsoft.Sql/servers@2023-05-01-preview' existing = {
  name: serverName
}

resource sqlDatabase 'Microsoft.Sql/servers/databases@2023-05-01-preview' = {
  parent: sqlServer
  name: name
  location: location
  sku: {
    name: sku
  }
  properties: {
    collation: 'SQL_Latin1_General_CP1_CI_AS'
    maxSizeBytes: sku == 'Basic' ? 2147483648 : 268435456000 // 2GB for Basic, 250GB for others
  }
}

output id string = sqlDatabase.id
output name string = sqlDatabase.name
