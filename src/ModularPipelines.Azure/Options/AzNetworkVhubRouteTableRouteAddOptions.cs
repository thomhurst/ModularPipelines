using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "vhub", "route-table", "route", "add")]
public record AzNetworkVhubRouteTableRouteAddOptions(
[property: CommandSwitch("--destination-type")] string DestinationType,
[property: CommandSwitch("--destinations")] string Destinations,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--next-hop-type")] string NextHopType,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--vhub-name")] string VhubName
) : AzOptions
{
    [CommandSwitch("--next-hop")]
    public string? NextHop { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--route-name")]
    public string? RouteName { get; set; }
}