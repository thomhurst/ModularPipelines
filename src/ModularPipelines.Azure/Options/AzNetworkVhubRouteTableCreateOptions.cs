using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network", "vhub", "route-table", "create")]
public record AzNetworkVhubRouteTableCreateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--vhub-name")] string VhubName
) : AzOptions
{
    [CliOption("--destination-type")]
    public string? DestinationType { get; set; }

    [CliOption("--destinations")]
    public string? Destinations { get; set; }

    [CliOption("--labels")]
    public string? Labels { get; set; }

    [CliOption("--next-hop")]
    public string? NextHop { get; set; }

    [CliOption("--next-hop-type")]
    public string? NextHopType { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--route-name")]
    public string? RouteName { get; set; }
}