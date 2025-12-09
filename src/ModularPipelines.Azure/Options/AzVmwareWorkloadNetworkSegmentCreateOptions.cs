using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("vmware", "workload-network", "segment", "create")]
public record AzVmwareWorkloadNetworkSegmentCreateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--private-cloud")] string PrivateCloud,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--connected-gateway")]
    public string? ConnectedGateway { get; set; }

    [CliOption("--dhcp-ranges")]
    public string? DhcpRanges { get; set; }

    [CliOption("--display-name")]
    public string? DisplayName { get; set; }

    [CliOption("--gateway-address")]
    public string? GatewayAddress { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--revision")]
    public string? Revision { get; set; }
}