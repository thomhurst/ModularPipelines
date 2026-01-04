using Azure.Core;
using Azure.Security.KeyVault.Certificates;
using Azure.Security.KeyVault.Keys;
using Azure.Security.KeyVault.Secrets;

namespace ModularPipelines.Azure;

/// <summary>
/// Provides access to Azure Key Vault clients with caching.
/// </summary>
public interface IAzureKeyVault
{
    /// <summary>
    /// Gets or creates a cached <see cref="SecretClient"/> for the specified vault.
    /// </summary>
    /// <param name="vaultUri">The URI of the Key Vault.</param>
    /// <param name="tokenCredential">The credential to use for authentication.</param>
    /// <returns>A cached <see cref="SecretClient"/> instance.</returns>
    SecretClient GetSecretClient(Uri vaultUri, TokenCredential tokenCredential);

    /// <summary>
    /// Gets or creates a cached <see cref="CertificateClient"/> for the specified vault.
    /// </summary>
    /// <param name="vaultUri">The URI of the Key Vault.</param>
    /// <param name="tokenCredential">The credential to use for authentication.</param>
    /// <returns>A cached <see cref="CertificateClient"/> instance.</returns>
    CertificateClient GetCertificateClient(Uri vaultUri, TokenCredential tokenCredential);

    /// <summary>
    /// Gets or creates a cached <see cref="KeyClient"/> for the specified vault.
    /// </summary>
    /// <param name="vaultUri">The URI of the Key Vault.</param>
    /// <param name="tokenCredential">The credential to use for authentication.</param>
    /// <returns>A cached <see cref="KeyClient"/> instance.</returns>
    KeyClient GetKeyClient(Uri vaultUri, TokenCredential tokenCredential);
}
