@description('The location used for all deployed resources')
param location string = resourceGroup().location

@description('Tags that will be applied to all resources')
param tags object = {}

var resourceToken = uniqueString(resourceGroup().id)

resource managedIdentity 'Microsoft.ManagedIdentity/userAssignedIdentities@2023-01-31' = {
  name: 'mi-${resourceToken}'
  location: location
  tags: tags
}

resource logAnalyticsWorkspace 'Microsoft.OperationalInsights/workspaces@2022-10-01' = {
  name: 'law-${resourceToken}'
  location: location
  properties: {
    sku: {
      name: 'PerGB2018'
    }
  }
  tags: tags
}

resource containerAppEnvironment 'Microsoft.App/managedEnvironments@2023-05-01' = {
  name: 'cae-${resourceToken}'
  location: location
  properties: {
    appLogsConfiguration: {
      destination: 'log-analytics'
      logAnalyticsConfiguration: {
        customerId: logAnalyticsWorkspace.properties.customerId
        sharedKey: logAnalyticsWorkspace.listKeys().primarySharedKey
      }
    }
  }
  tags: tags
}

resource seleniumHub 'Microsoft.App/containerApps@2023-05-02-preview' = {
  name: 'selenium-hub'
  location: location
  properties: {
    environmentId: containerAppEnvironment.id
    configuration: {
      activeRevisionsMode: 'Single'
      ingress: {
        external: true
        targetPort: 4444
        transport: 'http'
        allowInsecure: false
        additionalPortMappings: [
            {
              external: false
              exposedPort: 4443
              targetPort: 4443
            }
            {
              external: false
              exposedPort: 4442
              targetPort: 4442
            }
        ]
      }
    }
    template: {
      containers: [
        {
          image: 'selenium/hub:4.18.0-20240220'
          name: 'selenium-hub'
        }
      ]
      scale: {
        minReplicas: 1
      }
    }
  }
  tags: union(tags, {'aspire-resource-name': 'selenium-hub'})
}

resource seleniumNode 'Microsoft.App/containerApps@2023-05-02-preview' = {
  name: 'selenium-node'
  location: location
  dependsOn: [
     seleniumHub
  ]
  properties: {
     environmentId: containerAppEnvironment.id
     configuration: {
       activeRevisionsMode: 'Single'
     }
     template: {
       containers: [
         {
           image: 'selenium/node-firefox:4.18.0-20240220'
           name: 'selenium-node'
           env: [
             {
               name: 'SE_EVENT_BUS_HOST'
               value: seleniumHub.id
             }
             {
               name: 'SE_EVENT_BUS_PUBLISH_PORT'
               value: '4442'
             }
             {
               name: 'SE_EVENT_BUS_SUBSCRIBE_PORT'
               value: '4443'
             }
           ]
         }
       ]
     }
  }
}

output MANAGED_IDENTITY_CLIENT_ID string = managedIdentity.properties.clientId
output MANAGED_IDENTITY_NAME string = managedIdentity.name
output MANAGED_IDENTITY_PRINCIPAL_ID string = managedIdentity.properties.principalId
output AZURE_LOG_ANALYTICS_WORKSPACE_NAME string = logAnalyticsWorkspace.name
output AZURE_LOG_ANALYTICS_WORKSPACE_ID string = logAnalyticsWorkspace.id
output AZURE_CONTAINER_APPS_ENVIRONMENT_ID string = containerAppEnvironment.id
output AZURE_CONTAINER_APPS_ENVIRONMENT_DEFAULT_DOMAIN string = containerAppEnvironment.properties.defaultDomain
