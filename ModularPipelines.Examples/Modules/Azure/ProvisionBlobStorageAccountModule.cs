using Azure.Core;
using Azure.ResourceManager.Storage;
using Azure.ResourceManager.Storage.Models;
using ModularPipelines.Azure.Extensions;
using ModularPipelines.Azure.Scopes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Modules;

namespace ModularPipelines.Examples.Modules.Azure;

public class ProvisionBlobStorageAccountModule : Module<StorageAccountResource>
{
    protected override async Task<ModuleResult<StorageAccountResource>?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {
        var blobStorageAccountProvisionResponse = await context.Azure().Provisioner.Storage.StorageAccount(
            new AzureResourceIdentifier("MySubscription", "MyResourceGroup", "MyStorage"),
            new StorageAccountCreateOrUpdateContent(new StorageSku(StorageSkuName.StandardGrs), StorageKind.BlobStorage, AzureLocation.UKSouth)
        );
        
        return blobStorageAccountProvisionResponse.Value;
    }
}