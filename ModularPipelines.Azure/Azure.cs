using ModularPipelines.Azure.Provisioning;

namespace ModularPipelines.Azure;

public class Azure : IAzure
{
    public Azure(IAzureProvisioner azureProvisioner, IAzureKeyVault keyVault)
    {
        Provisioner = azureProvisioner;
        KeyVault = keyVault;
    }
    
    public IAzureProvisioner Provisioner { get; }
    public IAzureKeyVault KeyVault { get; }
}