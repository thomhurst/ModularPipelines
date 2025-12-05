using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("network", "vhub", "route-table", "route", "add")]
public record AzNetworkVhubRouteTableRouteAddOptions(
[property: CliOption("--destination-type")] string DestinationType,
[property: CliOption("--destinations")] string Destinations,
[property: CliOption("--name")] string Name,
[property: CliOption("--next-hop-type")] string NextHopType,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--vhub-name")] string VhubName
) : AzOptions
{
    [CliOption("--next-hop")]
    public string? NextHop { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--route-name")]
    public string? RouteName { get; set; }
}