using System.Collections.Concurrent;
using Azure.Core;
using Azure.Security.KeyVault.Certificates;
using Azure.Security.KeyVault.Keys;
using Azure.Security.KeyVault.Secrets;

namespace ModularPipelines.Azure;

/// <summary>
/// Provides access to Azure Key Vault clients with caching.
/// </summary>
/// <remarks>
/// Azure SDK clients (SecretClient, CertificateClient, KeyClient) are thread-safe and designed
/// to be long-lived and reused. This class caches clients by vault URI to avoid the overhead
/// of creating new clients on every call.
/// </remarks>
internal class AzureKeyVault : IAzureKeyVault
{
    // Use composite key (vaultUri, credential) to ensure different credentials for the same vault
    // return different clients. TokenCredential uses reference equality.
    private readonly ConcurrentDictionary<(string VaultUri, TokenCredential Credential), SecretClient> _secretClients = new();
    private readonly ConcurrentDictionary<(string VaultUri, TokenCredential Credential), CertificateClient> _certificateClients = new();
    private readonly ConcurrentDictionary<(string VaultUri, TokenCredential Credential), KeyClient> _keyClients = new();

    public SecretClient GetSecretClient(Uri vaultUri, TokenCredential tokenCredential)
    {
        var key = (vaultUri.ToString(), tokenCredential);
        return _secretClients.GetOrAdd(key, _ => new SecretClient(vaultUri, tokenCredential));
    }

    public CertificateClient GetCertificateClient(Uri vaultUri, TokenCredential tokenCredential)
    {
        var key = (vaultUri.ToString(), tokenCredential);
        return _certificateClients.GetOrAdd(key, _ => new CertificateClient(vaultUri, tokenCredential));
    }

    public KeyClient GetKeyClient(Uri vaultUri, TokenCredential tokenCredential)
    {
        var key = (vaultUri.ToString(), tokenCredential);
        return _keyClients.GetOrAdd(key, _ => new KeyClient(vaultUri, tokenCredential));
    }
}