using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "vhub", "route-map", "get-outbound-routes")]
public record AzNetworkVhubRouteMapGetOutboundRoutesOptions : AzOptions
{
    [CommandSwitch("--connection-type")]
    public string? ConnectionType { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--resource-uri")]
    public string? ResourceUri { get; set; }

    [CommandSwitch("--subscription")]
    public new string? Subscription { get; set; }

    [CommandSwitch("--vhub-name")]
    public string? VhubName { get; set; }
}