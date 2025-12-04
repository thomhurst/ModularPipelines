using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("network", "route-table", "route", "create")]
public record AzNetworkRouteTableRouteCreateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--route-table-name")] string RouteTableName
) : AzOptions
{
    [CliOption("--address-prefix")]
    public string? AddressPrefix { get; set; }

    [CliOption("--next-hop-ip-address")]
    public string? NextHopIpAddress { get; set; }

    [CliOption("--next-hop-type")]
    public string? NextHopType { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }
}