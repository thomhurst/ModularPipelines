using ModularPipelines.Azure.Provisioning;
using ModularPipelines.Azure.Services;

namespace ModularPipelines.Azure;

public interface IAzure
{
    IAzureProvisioner Provisioner { get; }

    IAzureKeyVault KeyVault { get; }

    Az Az { get; }
}