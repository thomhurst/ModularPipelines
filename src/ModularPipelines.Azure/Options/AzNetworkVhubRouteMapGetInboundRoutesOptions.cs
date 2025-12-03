using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network", "vhub", "route-map", "get-inbound-routes")]
public record AzNetworkVhubRouteMapGetInboundRoutesOptions : AzOptions
{
    [CliOption("--connection-type")]
    public string? ConnectionType { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--resource-uri")]
    public string? ResourceUri { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--vhub-name")]
    public string? VhubName { get; set; }
}