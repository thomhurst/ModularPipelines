using Azure.Core;
using Azure.ResourceManager.AppService;
using Azure.ResourceManager.AppService.Models;
using Azure.ResourceManager.ManagedServiceIdentities;
using Azure.ResourceManager.Models;
using Azure.ResourceManager.Storage;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Extensions;
using ModularPipelines.Azure.Scopes;
using ModularPipelines.Context;
using ModularPipelines.Modules;

namespace ModularPipelines.Examples.Modules.Azure;

[DependsOn<ProvisionUserAssignedIdentityModule>]
[DependsOn<ProvisionBlobStorageAccountModule>]
[DependsOn<ProvisionBlobStorageContainerModule>]
public class ProvisionAzureFunction : IModule<WebSiteResource>
{
    /// <inheritdoc/>
    public async Task<WebSiteResource?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {
        var userAssignedIdentity = context.GetModule<ProvisionUserAssignedIdentityModule, UserAssignedIdentityResource>();

        var storageAccount = context.GetModule<ProvisionBlobStorageAccountModule, StorageAccountResource>();
        var blobContainer = context.GetModule<ProvisionBlobStorageContainerModule, BlobContainerResource>();

        var functionProvisionResponse = await context.Azure().Provisioner.Compute.WebSite(
            new AzureResourceIdentifier("MySubscription", "MyResourceGroup", "MyFunction"),
            new WebSiteData(AzureLocation.UKSouth)
            {
                Identity = new ManagedServiceIdentity(ManagedServiceIdentityType.UserAssigned)
                {
                    UserAssignedIdentities = { { userAssignedIdentity.Value!.Id, new UserAssignedIdentity() } },
                },
                SiteConfig = new SiteConfigProperties
                {
                    AppSettings = new List<AppServiceNameValuePair>
                    {
                        new()
                        {
                            Name = "BlobStorageConnectionString",
                            Value = storageAccount.Value!.Data.PrimaryEndpoints.BlobUri.AbsoluteUri,
                        },
                        new()
                        {
                            Name = "BlobContainerName",
                            Value = blobContainer.Value!.Data.Name,
                        },
                    },
                },

                // ... Other properties
            }
        );

        return functionProvisionResponse.Value;
    }
}