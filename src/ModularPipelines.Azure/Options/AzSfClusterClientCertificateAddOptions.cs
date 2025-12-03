using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sf", "cluster", "client-certificate", "add")]
public record AzSfClusterClientCertificateAddOptions(
[property: CliOption("--cluster-name")] string ClusterName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--admin-client-thumbprints")]
    public string? AdminClientThumbprints { get; set; }

    [CliOption("--cert-common-name")]
    public string? CertCommonName { get; set; }

    [CliOption("--cert-issuer-tp")]
    public string? CertIssuerTp { get; set; }

    [CliOption("--client-cert-cn")]
    public string? ClientCertCn { get; set; }

    [CliFlag("--is-admin")]
    public bool? IsAdmin { get; set; }

    [CliOption("--readonly-client-thumbprints")]
    public string? ReadonlyClientThumbprints { get; set; }

    [CliOption("--thumbprint")]
    public string? Thumbprint { get; set; }
}