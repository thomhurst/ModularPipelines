using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("sf", "managed-cluster", "update")]
public record AzSfManagedClusterUpdateOptions(
[property: CliOption("--cluster-name")] string ClusterName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--client-connection-port")]
    public string? ClientConnectionPort { get; set; }

    [CliOption("--dns-name")]
    public string? DnsName { get; set; }

    [CliOption("--gateway-connection-port")]
    public string? GatewayConnectionPort { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }
}