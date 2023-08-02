using Azure.ResourceManager;
using Azure.ResourceManager.Resources;
using ModularPipelines.Azure.Scopes;

namespace ModularPipelines.Azure.Provisioning;

public abstract class BaseAzureProvisioner
{
    public ArmClient ArmClient { get; }

    protected BaseAzureProvisioner(ArmClient armClient)
    {
        ArmClient = armClient;
    }

    public ResourceGroupResource GetResourceGroup(AzureResourceIdentifier resourceIdentifier) =>
        ArmClient.GetResourceGroupResource(resourceIdentifier.ToResourceGroupIdentifier());
}