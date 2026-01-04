using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Azure.Provisioning;
using ModularPipelines.Azure.Services;

namespace ModularPipelines.Azure;

/// <summary>
/// Provides access to Azure services and provisioning.
/// </summary>
[ExcludeFromCodeCoverage]
public class Azure : IAzure
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Azure"/> class.
    /// </summary>
    public Azure(IAzureProvisioner azureProvisioner, IAzureKeyVault keyVault, IAz az)
    {
        Provisioner = azureProvisioner;
        KeyVault = keyVault;
        Az = az;
    }

    /// <inheritdoc/>
    public IAzureProvisioner Provisioner { get; }

    /// <inheritdoc/>
    public IAzureKeyVault KeyVault { get; }

    /// <inheritdoc/>
    public IAz Az { get; }
}
