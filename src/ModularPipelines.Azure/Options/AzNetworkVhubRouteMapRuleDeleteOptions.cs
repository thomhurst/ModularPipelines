using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network", "vhub", "route-map", "rule", "delete")]
public record AzNetworkVhubRouteMapRuleDeleteOptions(
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--route-map-name")] string RouteMapName,
[property: CliOption("--rule-index")] string RuleIndex,
[property: CliOption("--vhub-name")] string VhubName
) : AzOptions
{
    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliFlag("--yes")]
    public bool? Yes { get; set; }
}