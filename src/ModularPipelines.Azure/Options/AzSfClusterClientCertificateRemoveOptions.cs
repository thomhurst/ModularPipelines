using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("sf", "cluster", "client-certificate", "remove")]
public record AzSfClusterClientCertificateRemoveOptions(
[property: CliOption("--cluster-name")] string ClusterName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--cert-common-name")]
    public string? CertCommonName { get; set; }

    [CliOption("--cert-issuer-tp")]
    public string? CertIssuerTp { get; set; }

    [CliOption("--client-cert-cn")]
    public string? ClientCertCn { get; set; }

    [CliOption("--thumbprints")]
    public string? Thumbprints { get; set; }
}