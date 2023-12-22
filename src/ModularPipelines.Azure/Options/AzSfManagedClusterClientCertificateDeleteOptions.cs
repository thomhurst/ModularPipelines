using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sf", "managed-cluster", "client-certificate", "delete")]
public record AzSfManagedClusterClientCertificateDeleteOptions(
[property: CommandSwitch("--cluster-name")] string ClusterName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--common-name")]
    public string? CommonName { get; set; }

    [CommandSwitch("--thumbprint")]
    public string? Thumbprint { get; set; }
}