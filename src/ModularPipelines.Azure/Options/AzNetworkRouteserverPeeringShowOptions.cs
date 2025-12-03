using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network", "routeserver", "peering", "show")]
public record AzNetworkRouteserverPeeringShowOptions : AzOptions
{
    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--routeserver")]
    public string? Routeserver { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}