using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sf", "cluster", "client-certificate", "add")]
public record AzSfClusterClientCertificateAddOptions(
[property: CommandSwitch("--cluster-name")] string ClusterName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--admin-client-thumbprints")]
    public string? AdminClientThumbprints { get; set; }

    [CommandSwitch("--cert-common-name")]
    public string? CertCommonName { get; set; }

    [CommandSwitch("--cert-issuer-tp")]
    public string? CertIssuerTp { get; set; }

    [CommandSwitch("--client-cert-cn")]
    public string? ClientCertCn { get; set; }

    [BooleanCommandSwitch("--is-admin")]
    public bool? IsAdmin { get; set; }

    [CommandSwitch("--readonly-client-thumbprints")]
    public string? ReadonlyClientThumbprints { get; set; }

    [CommandSwitch("--thumbprint")]
    public string? Thumbprint { get; set; }
}