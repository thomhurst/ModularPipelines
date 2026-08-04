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
    protected override async Task<UserAssignedIdentityResource> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {
        var userAssignedIdentityProvisionResponse = await context.Tools.Azure.Provisioner.Security.UserAssignedIdentityAsync(
            new AzureResourceIdentifier("MySubscription", "MyResourceGroup", "MyUserIdentity"),
            new UserAssignedIdentityData(AzureLocation.UKSouth),
            cancellationToken
        );

        return userAssignedIdentityProvisionResponse.Value;
    }
}
```

## Blob Storage Account

```csharp
public class ProvisionBlobStorageAccountModule : Module<StorageAccountResource>
{
    protected override async Task<StorageAccountResource> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {
        var blobStorageAccountProvisionResponse = await context.Tools.Azure.Provisioner.Storage.StorageAccountAsync(
            new AzureResourceIdentifier("MySubscription", "MyResourceGroup", "MyStorage"),
            new StorageAccountCreateOrUpdateContent(new StorageSku(StorageSkuName.StandardGrs), StorageKind.BlobStorage, AzureLocation.UKSouth),
            cancellationToken
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
    protected override async Task<BlobContainerResource> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {
        var blobStorageAccount = await context.GetModule<ProvisionBlobStorageAccountModule>();

        var blobContainerProvisionResponse = await context.Tools.Azure.Provisioner.Storage.BlobContainerAsync(
            blobStorageAccount.Value.Id,
            "MyContainer",
            new BlobContainerData(),
            cancellationToken
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
    protected override async Task<RoleAssignmentResource> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {
        var userAssignedIdentity = await context.GetModule<ProvisionUserAssignedIdentityModule>();

        var storageAccount = await context.GetModule<ProvisionBlobStorageAccountModule>();

        var roleAssignmentResource = await context.Tools.Azure.Provisioner.Security.RoleAssignmentAsync(
            storageAccount.Value.Id,
            new RoleAssignmentCreateOrUpdateContent(WellKnownRoleDefinitions.BlobStorageOwnerDefinitionId, userAssignedIdentity.Value.Data.PrincipalId!.Value),
            cancellationToken
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
    protected override async Task<WebSiteResource> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {
        var userAssignedIdentity = await context.GetModule<ProvisionUserAssignedIdentityModule>();

        var storageAccount = await context.GetModule<ProvisionBlobStorageAccountModule>();
        var blobContainer = await context.GetModule<ProvisionBlobStorageContainerModule>();

        var functionProvisionResponse = await context.Tools.Azure.Provisioner.Compute.WebSiteAsync(
            new AzureResourceIdentifier("MySubscription", "MyResourceGroup", "MyFunction"),
            new WebSiteData(AzureLocation.UKSouth)
            {
                Identity = new ManagedServiceIdentity(ManagedServiceIdentityType.UserAssigned)
                {
                    UserAssignedIdentities = { { userAssignedIdentity.Value.Id, new UserAssignedIdentity() } }
                },
                SiteConfig = new SiteConfigProperties
                {
                    AppSettings = new List<AppServiceNameValuePair>
                    {
                        new()
                        {
                            Name = "BlobStorageConnectionString",
                            Value = storageAccount.Value.Data.PrimaryEndpoints.BlobUri.AbsoluteUri
                        },
                        new()
                        {
                            Name = "BlobContainerName",
                            Value = blobContainer.Value.Data.Name
                        }
                    }
                }
                // ... Other properties
            },
            cancellationToken
        );

        return functionProvisionResponse.Value;
    }
```

So we can clearly see what depends on what. 

Every module is isolated. 

Every module will automatically run in parallel unless it is dependent on another module.

A module with a dependency can retrieve the data from another module.
