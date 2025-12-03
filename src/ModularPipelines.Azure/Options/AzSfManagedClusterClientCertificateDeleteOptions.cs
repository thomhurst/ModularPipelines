using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sf", "managed-cluster", "client-certificate", "delete")]
public record AzSfManagedClusterClientCertificateDeleteOptions(
[property: CliOption("--cluster-name")] string ClusterName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--common-name")]
    public string? CommonName { get; set; }

    [CliOption("--thumbprint")]
    public string? Thumbprint { get; set; }
}