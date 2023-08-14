using Azure.ResourceManager.Storage;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Extensions;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Modules;

namespace ModularPipelines.Examples.Modules.Azure;

[DependsOn<ProvisionBlobStorageAccountModule>]
public class ProvisionBlobStorageContainerModule : Module<BlobContainerResource>
{
    protected override async Task<ModuleResult<BlobContainerResource>?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
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