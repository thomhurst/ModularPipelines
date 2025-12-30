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
/// <para>
/// Azure SDK clients (SecretClient, CertificateClient, KeyClient) are thread-safe and designed
/// to be long-lived and reused. This class caches clients by vault URI to avoid the overhead
/// of creating new clients on every call.
/// </para>
/// <para>
/// <strong>Important:</strong> Clients are cached by vault URI only. Within a scope, the first
/// TokenCredential used for a vault URI will be used for all subsequent requests to that vault.
/// This is intentional since AzureKeyVault is registered as Scoped and typically the same
/// credential is used throughout a pipeline execution scope.
/// </para>
/// </remarks>
internal class AzureKeyVault : IAzureKeyVault
{
    // Cache by vault URI only. Within a scope, the same credential is typically used.
    // TokenCredential lacks value equality, so including it in the key would cause cache misses
    // for logically identical credentials (e.g., two DefaultAzureCredential instances).
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