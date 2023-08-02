using Azure.Core;
using Azure.Security.KeyVault.Certificates;
using Azure.Security.KeyVault.Keys;
using Azure.Security.KeyVault.Secrets;

namespace ModularPipelines.Azure;

internal class AzureKeyVault : IAzureKeyVault
{
    public SecretClient GetSecretClient(Uri vaultUri, TokenCredential tokenCredential)
    {
        return new SecretClient(vaultUri, tokenCredential);
    }
    
    public CertificateClient GetCertificateClient(Uri vaultUri, TokenCredential tokenCredential)
    {
        return new CertificateClient(vaultUri, tokenCredential);
    }
    
    public KeyClient GetKeyClient(Uri vaultUri, TokenCredential tokenCredential)
    {
        return new KeyClient(vaultUri, tokenCredential);
    }
}