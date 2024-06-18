
param location string = resourceGroup().location

var resourceToken = uniqueString(resourceGroup().id)

resource containerAppEnvironment 'Microsoft.App/managedEnvironments@2023-05-01' existing = {
  name: 'cae-${resourceToken}'
  scope: resourceGroup()
}

resource nginxProxy 'Microsoft.App/containerApps@2023-05-02-preview' = {
  name: 'nginx-proxy'
  location: location
  properties: {
     environmentId: containerAppEnvironment.id
     configuration: {
       activeRevisionsMode: 'Single'
       ingress: {
         external: true
         targetPort: 80
         transport: 'http'
         allowInsecure: false
       }
     }
    template: {
       containers: [
         {
           image: 'crbbdev.azurecr.io/betterboard/nginx-proxy:latest'
           name: 'nginx-proxy'
         }
       ]
       scale: {
         minReplicas: 0
         maxReplicas: 1
       }
    }
  }
}
