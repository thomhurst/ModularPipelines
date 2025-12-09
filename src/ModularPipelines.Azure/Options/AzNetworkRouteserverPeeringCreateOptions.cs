using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("network", "routeserver", "peering", "create")]
public record AzNetworkRouteserverPeeringCreateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--routeserver")] string Routeserver
) : AzOptions
{
    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--peer-asn")]
    public string? PeerAsn { get; set; }

    [CliOption("--peer-ip")]
    public string? PeerIp { get; set; }
}