# Example deployment command

```bash
az deployment group create --resource-group exampleRG --template-file keyvault.bicep --parameters keyVaultName=<vault-name> objectId=<object-id>
```

Replace <vault-name> with the name of the key vault. Replace <object-id> with the object ID of a user, service principal, or security group in the Microsoft Entra tenant for the vault. The object ID must be unique for the list of access policies.