using ModularPipelines.Azure.Provisioning;

namespace ModularPipelines.Azure;

public class Azure : IAzure
{
    public Azure(IAzureProvisioner azureProvisioner, IAzureKeyVault keyVault)
    {
        Provisioner = azureProvisioner;
        KeyVault = keyVault;
    }

    /// <inheritdoc/>
    public IAzureProvisioner Provisioner { get; }

    /// <inheritdoc/>
    public IAzureKeyVault KeyVault { get; }
}