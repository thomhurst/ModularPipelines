using Azure.Core;
using Azure.ResourceManager.AppService;
using Azure.ResourceManager.AppService.Models;
using Azure.ResourceManager.Models;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Extensions;
using ModularPipelines.Azure.Scopes;
using ModularPipelines.Context;
using ModularPipelines.Modules;

namespace ModularPipelines.Examples.Modules.Azure;

[DependsOn<ProvisionUserAssignedIdentityModule>]
[DependsOn<ProvisionBlobStorageAccountModule>]
[DependsOn<ProvisionBlobStorageContainerModule>]
public class ProvisionAzureFunction : ModuleNew<WebSiteResource>
{
    /// <inheritdoc/>
    public override async Task<WebSiteResource?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
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