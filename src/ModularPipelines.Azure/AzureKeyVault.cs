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
    private readonly ConcurrentDictionary<string, SecretClient> _secretClients = new();
    private readonly ConcurrentDictionary<string, CertificateClient> _certificateClients = new();
    private readonly ConcurrentDictionary<string, KeyClient> _keyClients = new();

    public SecretClient GetSecretClient(Uri vaultUri, TokenCredential tokenCredential)
    {
        var key = vaultUri.ToString();
        return _secretClients.GetOrAdd(key, _ => new SecretClient(vaultUri, tokenCredential));
    }

    public CertificateClient GetCertificateClient(Uri vaultUri, TokenCredential tokenCredential)
    {
        var key = vaultUri.ToString();
        return _certificateClients.GetOrAdd(key, _ => new CertificateClient(vaultUri, tokenCredential));
    }

    public KeyClient GetKeyClient(Uri vaultUri, TokenCredential tokenCredential)
    {
        var key = vaultUri.ToString();
        return _keyClients.GetOrAdd(key, _ => new KeyClient(vaultUri, tokenCredential));
    }
}