using Azure.Core;
using Azure.ResourceManager.Storage;
using Azure.ResourceManager.Storage.Models;
using ModularPipelines.Azure.Extensions;
using ModularPipelines.Azure.Scopes;
using ModularPipelines.Context;
using ModularPipelines.Modules;

namespace ModularPipelines.Examples.Modules.Azure;

public class ProvisionBlobStorageAccountModule : IModule<StorageAccountResource>
{
    /// <inheritdoc/>
    public async Task<StorageAccountResource?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {
        var blobStorageAccountProvisionResponse = await context.Azure().Provisioner.Storage.StorageAccount(
            new AzureResourceIdentifier("MySubscription", "MyResourceGroup", "MyStorage"),
            new StorageAccountCreateOrUpdateContent(new StorageSku(StorageSkuName.StandardGrs), StorageKind.BlobStorage, AzureLocation.UKSouth)
        );

        return blobStorageAccountProvisionResponse.Value;
    }
}