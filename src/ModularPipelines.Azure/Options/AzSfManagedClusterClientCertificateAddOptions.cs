using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sf", "managed-cluster", "client-certificate", "add")]
public record AzSfManagedClusterClientCertificateAddOptions(
[property: CliOption("--cluster-name")] string ClusterName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--common-name")]
    public string? CommonName { get; set; }

    [CliFlag("--is-admin")]
    public bool? IsAdmin { get; set; }

    [CliOption("--issuer-thumbprint")]
    public string? IssuerThumbprint { get; set; }

    [CliOption("--thumbprint")]
    public string? Thumbprint { get; set; }
}