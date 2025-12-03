using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network", "vhub", "route-map", "rule", "show")]
public record AzNetworkVhubRouteMapRuleShowOptions(
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--route-map-name")] string RouteMapName,
[property: CliOption("--rule-index")] string RuleIndex,
[property: CliOption("--vhub-name")] string VhubName
) : AzOptions;