using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "route-table", "route", "create")]
public record AzNetworkRouteTableRouteCreateOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--route-table-name")] string RouteTableName
) : AzOptions
{
    [CommandSwitch("--address-prefix")]
    public string? AddressPrefix { get; set; }

    [CommandSwitch("--next-hop-ip-address")]
    public string? NextHopIpAddress { get; set; }

    [CommandSwitch("--next-hop-type")]
    public string? NextHopType { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }
}

