using Azure;
using Azure.ResourceManager;
using Azure.ResourceManager.AppService;
using ModularPipelines.Azure.Scopes;

namespace ModularPipelines.Azure.Provisioning.Compute;

public class AzureKubernetesProvisioner : BaseAzureProvisioner
{
    public AzureKubernetesProvisioner(ArmClient armClient) : base(armClient)
    {
    }

    public async Task<ArmOperation<KubeEnvironmentResource>> KubeEnvironment(AzureResourceIdentifier azureResourceIdentifier, KubeEnvironmentData properties, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(azureResourceIdentifier);
        ArgumentNullException.ThrowIfNull(properties);
        ArgumentException.ThrowIfNullOrWhiteSpace(azureResourceIdentifier.ResourceName);

        return await GetResourceGroup(azureResourceIdentifier).GetKubeEnvironments()
            .CreateOrUpdateAsync(WaitUntil.Completed, azureResourceIdentifier.ResourceName, properties, cancellationToken);
    }
}
