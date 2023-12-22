using Azure.Core;
using Azure.Security.KeyVault.Certificates;
using Azure.Security.KeyVault.Keys;
using Azure.Security.KeyVault.Secrets;

namespace ModularPipelines.Azure;

public interface IAzureKeyVault
{
    SecretClient GetSecretClient(Uri vaultUri, TokenCredential tokenCredential);

    CertificateClient GetCertificateClient(Uri vaultUri, TokenCredential tokenCredential);

    KeyClient GetKeyClient(Uri vaultUri, TokenCredential tokenCredential);
}