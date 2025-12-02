using Azure.ResourceManager.Storage;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Extensions;
using ModularPipelines.Context;
using ModularPipelines.Modules;

namespace ModularPipelines.Examples.Modules.Azure;

[DependsOn<ProvisionBlobStorageAccountModule>]
public class ProvisionBlobStorageContainerModule : Module<BlobContainerResource>
{
    /// <inheritdoc/>
    public override async Task<BlobContainerResource?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {
        var blobStorageAccount = context.GetModule<ProvisionBlobStorageAccountModule, StorageAccountResource>();

        var blobContainerProvisionResponse = await context.Azure().Provisioner.Storage.BlobContainer(
            blobStorageAccount.Value!.Id,
            "MyContainer",
            new BlobContainerData()
        );

        return blobContainerProvisionResponse.Value;
    }
}