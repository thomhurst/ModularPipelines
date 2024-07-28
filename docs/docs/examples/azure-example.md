---
title: Azure Resource Provisioning Example
---

# Azure Resource Provisioning Example

So for example, we want to provision some Azure services like this:

- A user assigned identity
- A blob storage account that can only be accessed via the user assigned identity we created
- A blob storage container under that account
- An azure function, with our user assigned identity being used for its identity, meaning it would have access to the blob storage

That would look like this:

## Managed User Assigned Identity

```csharp
public class ProvisionUserAssignedIdentityModule : Module<UserAssignedIdentityResource>
{
    protected override async Task<UserAssignedIdentityResource?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
    {
        var userAssignedIdentityProvisionResponse = await context.Azure().Provisioner.Security.UserAssignedIdentity(
            new AzureResourceIdentifier("MySubscription", "MyResourceGroup", "MyUserIdentity"),
            new UserAssignedIdentityData(AzureLocation.UKSouth)
        );
        
        return userAssignedIdentityProvisionResponse.Value;
    }
}
```

## Blob Storage Account

```csharp
public class ProvisionBlobStorageAccountModule : Module<StorageAccountResource>
{
    protected override async Task<StorageAccountResource?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
    {
        var blobStorageAccountProvisionResponse = await context.Azure().Provisioner.Storage.StorageAccount(
            new AzureResourceIdentifier("MySubscription", "MyResourceGroup", "MyStorage"),
            new StorageAccountCreateOrUpdateContent(new StorageSku(StorageSkuName.StandardGrs), StorageKind.BlobStorage, AzureLocation.UKSouth)
        );
        
        return blobStorageAccountProvisionResponse.Value;
    }
}
```

## Blob Storage Container

```csharp
[DependsOn<ProvisionBlobStorageAccountModule>]
public class ProvisionBlobStorageContainerModule : Module<BlobContainerResource>
{
    protected override async Task<BlobContainerResource?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
    {
        var blobStorageAccount = await GetModule<ProvisionBlobStorageAccountModule>();

        var blobContainerProvisionResponse = await context.Azure().Provisioner.Storage.BlobContainer(
            blobStorageAccount.Value!.Id,
            "MyContainer",
            new BlobContainerData()
        );
        
        return blobContainerProvisionResponse.Value;
    }
}
```

## Blob Storage Access via User Identity

```csharp
[DependsOn<ProvisionBlobStorageAccountModule>]
[DependsOn<ProvisionUserAssignedIdentityModule>]
public class AssignAccessToBlobStorageModule : Module<RoleAssignmentResource>
{
    protected override async Task<RoleAssignmentResource?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
    {
        var userAssignedIdentity = await GetModule<ProvisionUserAssignedIdentityModule>();
        
        var storageAccount = await GetModule<ProvisionBlobStorageAccountModule>();
        
        var roleAssignmentResource = await context.Azure().Provisioner.Security.RoleAssignment(
            storageAccount.Value!.Id,
            new RoleAssignmentCreateOrUpdateContent(WellKnownRoleDefinitions.BlobStorageOwnerDefinitionId, userAssignedIdentity.Value!.Data.PrincipalId!.Value)
        );
        
        return roleAssignmentResource.Value;
    }
}
```

## Azure Function

```csharp
[DependsOn<ProvisionUserAssignedIdentityModule>]
[DependsOn<ProvisionBlobStorageAccountModule>]
[DependsOn<ProvisionBlobStorageContainerModule>]
public class ProvisionAzureFunction : Module<WebSiteResource>
{
    protected override async Task<WebSiteResource?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
    {
        var userAssignedIdentity = await GetModule<ProvisionUserAssignedIdentityModule>();

        var storageAccount = await GetModule<ProvisionBlobStorageAccountModule>();
        var blobContainer = await GetModule<ProvisionBlobStorageContainerModule>();
        
        var functionProvisionResponse = await context.Azure().Provisioner.Compute.WebSite(
            new AzureResourceIdentifier("MySubscription", "MyResourceGroup", "MyFunction"),
            new WebSiteData(AzureLocation.UKSouth)
            {
                Identity = new ManagedServiceIdentity(ManagedServiceIdentityType.UserAssigned)
                {
                    UserAssignedIdentities = { { userAssignedIdentity.Value!.Id, new UserAssignedIdentity() } }
                },
                SiteConfig = new SiteConfigProperties
                {
                    AppSettings = new List<AppServiceNameValuePair>
                    {
                        new()
                        {
                            Name = "BlobStorageConnectionString",
                            Value = storageAccount.Value!.Data.PrimaryEndpoints.BlobUri.AbsoluteUri
                        },
                        new()
                        {
                            Name = "BlobContainerName",
                            Value = blobContainer.Value!.Data.Name
                        }
                    }
                }
                // ... Other properties
            }
        );
        
        return functionProvisionResponse.Value;
    }
```

So we can clearly see what depends on what. 

Every module is isolated. 

Every module will automatically run in parallel unless it is dependent on another module.

A module with a dependency can retrieve the data from another module.