using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sf", "cluster", "client-certificate", "remove")]
public record AzSfClusterClientCertificateRemoveOptions(
[property: CommandSwitch("--cluster-name")] string ClusterName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--cert-common-name")]
    public string? CertCommonName { get; set; }

    [CommandSwitch("--cert-issuer-tp")]
    public string? CertIssuerTp { get; set; }

    [CommandSwitch("--client-cert-cn")]
    public string? ClientCertCn { get; set; }

    [CommandSwitch("--thumbprints")]
    public string? Thumbprints { get; set; }
}

