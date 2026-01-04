using ModularPipelines.Azure.Provisioning;
using ModularPipelines.Azure.Services;

namespace ModularPipelines.Azure;

/// <summary>
/// Provides access to Azure services and provisioning.
/// </summary>
public interface IAzure
{
    /// <summary>
    /// Gets the Azure resource provisioner.
    /// </summary>
    IAzureProvisioner Provisioner { get; }

    /// <summary>
    /// Gets the Azure Key Vault client factory.
    /// </summary>
    IAzureKeyVault KeyVault { get; }

    /// <summary>
    /// Gets the Azure CLI (az) commands service.
    /// </summary>
    IAz Az { get; }
}
