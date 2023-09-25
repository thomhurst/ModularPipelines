using ModularPipelines.Azure.Provisioning;

namespace ModularPipelines.Azure;

public interface IAzure
{
    IAzureProvisioner Provisioner { get; }

    IAzureKeyVault KeyVault { get; }
}