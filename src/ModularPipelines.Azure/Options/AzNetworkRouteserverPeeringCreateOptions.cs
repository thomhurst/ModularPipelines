using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "routeserver", "peering", "create")]
public record AzNetworkRouteserverPeeringCreateOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--routeserver")] string Routeserver
) : AzOptions
{
    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--peer-asn")]
    public string? PeerAsn { get; set; }

    [CommandSwitch("--peer-ip")]
    public string? PeerIp { get; set; }
}