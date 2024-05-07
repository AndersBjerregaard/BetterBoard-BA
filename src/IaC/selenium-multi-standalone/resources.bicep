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

resource standaloneChrome 'Microsoft.App/containerApps@2023-05-02-preview' = {
  name: 'standalone-chrome'
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
      }
    }
    template: {
      containers: [
        {
          image: 'selenium/standalone-chrome:4.18.0-20240220'
          name: 'standalone-chrome'
          env: [
            {
              name: 'SE_NODE_MAX_SESSIONS'
              value: '3'
            }
            {
              name: 'SE_NODE_OVERRIDE_MAX_SESSIONS'
              value: 'true'
            }
            {
              name: 'SE_NODE_SESSION_TIMEOUT'
              value: '60'
            }
          ]
        }
      ]
      scale: {
        minReplicas: 0
      }
    }
  }
  tags: union(tags, {'aspire-resource-name': 'standalone-chrome'})
}

resource standaloneEdge 'Microsoft.App/containerApps@2023-05-02-preview' = {
  name: 'standalone-edge'
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
      }
    }
    template: {
      containers: [
        {
          image: 'selenium/standalone-edge:4.18.0-20240220'
          name: 'standalone-edge'
          env: [
            {
              name: 'SE_NODE_MAX_SESSIONS'
              value: '3'
            }
            {
              name: 'SE_NODE_OVERRIDE_MAX_SESSIONS'
              value: 'true'
            }
            {
              name: 'SE_NODE_SESSION_TIMEOUT'
              value: '60'
            }
          ]
        }
      ]
      scale: {
        minReplicas: 0
      }
    }
  }
  tags: union(tags, {'aspire-resource-name': 'standalone-edge'})
}

resource standaloneFirefox 'Microsoft.App/containerApps@2023-05-02-preview' = {
  name: 'standalone-firefox'
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
      }
    }
    template: {
      containers: [
        {
          image: 'selenium/standalone-firefox:4.18.0-20240220'
          name: 'standalone-firefox'
          env: [
            {
              name: 'SE_NODE_MAX_SESSIONS'
              value: '3'
            }
            {
              name: 'SE_NODE_OVERRIDE_MAX_SESSIONS'
              value: 'true'
            }
            {
              name: 'SE_NODE_SESSION_TIMEOUT'
              value: '60'
            }
          ]
        }
      ]
      scale: {
        minReplicas: 0
      }
    }
  }
  tags: union(tags, {'aspire-resource-name': 'standalone-firefox'})
}

output MANAGED_IDENTITY_CLIENT_ID string = managedIdentity.properties.clientId
output MANAGED_IDENTITY_NAME string = managedIdentity.name
output MANAGED_IDENTITY_PRINCIPAL_ID string = managedIdentity.properties.principalId
output AZURE_LOG_ANALYTICS_WORKSPACE_NAME string = logAnalyticsWorkspace.name
output AZURE_LOG_ANALYTICS_WORKSPACE_ID string = logAnalyticsWorkspace.id
output AZURE_CONTAINER_APPS_ENVIRONMENT_ID string = containerAppEnvironment.id
output AZURE_CONTAINER_APPS_ENVIRONMENT_DEFAULT_DOMAIN string = containerAppEnvironment.properties.defaultDomain
