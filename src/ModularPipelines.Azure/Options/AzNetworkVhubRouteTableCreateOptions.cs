using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "vhub", "route-table", "create")]
public record AzNetworkVhubRouteTableCreateOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--vhub-name")] string VhubName
) : AzOptions
{
    [CommandSwitch("--destination-type")]
    public string? DestinationType { get; set; }

    [CommandSwitch("--destinations")]
    public string? Destinations { get; set; }

    [CommandSwitch("--labels")]
    public string? Labels { get; set; }

    [CommandSwitch("--next-hop")]
    public string? NextHop { get; set; }

    [CommandSwitch("--next-hop-type")]
    public string? NextHopType { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--route-name")]
    public string? RouteName { get; set; }
}