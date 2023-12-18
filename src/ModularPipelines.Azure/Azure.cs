using ModularPipelines.Azure.Provisioning;
using ModularPipelines.Azure.Services;

namespace ModularPipelines.Azure;

public class Azure : IAzure
{
    public Azure(IAzureProvisioner azureProvisioner, IAzureKeyVault keyVault, Az az)
    {
        Provisioner = azureProvisioner;
        KeyVault = keyVault;
        Az = az;
    }

    /// <inheritdoc/>
    public IAzureProvisioner Provisioner { get; }

    /// <inheritdoc/>
    public IAzureKeyVault KeyVault { get; }

    public Az Az { get; }
}