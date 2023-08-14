using Azure;
using Azure.ResourceManager;
using Azure.ResourceManager.ContainerRegistry;
using ModularPipelines.Azure.Scopes;

namespace ModularPipelines.Azure.Provisioning;

public class AzureContainerProvisioner : BaseAzureProvisioner
{
    public AzureContainerProvisioner(ArmClient armClient) : base(armClient)
    {
    }

    public async Task<ArmOperation<ContainerRegistryResource>> ContainerRegistry(AzureResourceIdentifier azureResourceIdentifier, ContainerRegistryData properties)
    {
        return await GetResourceGroup(azureResourceIdentifier).GetContainerRegistries()
            .CreateOrUpdateAsync(WaitUntil.Completed, azureResourceIdentifier.ResourceName, properties);
    }
}