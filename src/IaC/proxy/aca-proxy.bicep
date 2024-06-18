
param location string = resourceGroup().location

@description('Name of the container registry server that hosts the image')
param registryServer string

@secure()
param registryUsername string

@secure()
param registryPassword string

var registryPasswordSecret = 'registrypasswordsecretref'

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
         targetPort: 8080
         transport: 'http'
         allowInsecure: false
       }
       secrets: [
         {
           name: registryPasswordSecret
           value: registryPassword
         }
       ]
       registries: [
         {
           server: registryServer
           username: registryUsername
           passwordSecretRef: registryPasswordSecret
         }
       ]
     }
    template: {
       containers: [
         {
           image: 'crbbdev.azurecr.io/betterboard/nginx-proxy:latest'
           name: 'nginx-proxy'
           resources: {
             cpu: json('0.25')
             memory: '0.5Gi'
           }
         }
       ]
       scale: {
         minReplicas: 0
         maxReplicas: 1
       }
    }
  }
}
