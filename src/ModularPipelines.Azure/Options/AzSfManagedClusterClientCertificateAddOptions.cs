using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sf", "managed-cluster", "client-certificate", "add")]
public record AzSfManagedClusterClientCertificateAddOptions(
[property: CommandSwitch("--cluster-name")] string ClusterName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--common-name")]
    public string? CommonName { get; set; }

    [BooleanCommandSwitch("--is-admin")]
    public bool? IsAdmin { get; set; }

    [CommandSwitch("--issuer-thumbprint")]
    public string? IssuerThumbprint { get; set; }

    [CommandSwitch("--thumbprint")]
    public string? Thumbprint { get; set; }
}