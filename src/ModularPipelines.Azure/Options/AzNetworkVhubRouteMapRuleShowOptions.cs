using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "vhub", "route-map", "rule", "show")]
public record AzNetworkVhubRouteMapRuleShowOptions(
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--route-map-name")] string RouteMapName,
[property: CommandSwitch("--rule-index")] string RuleIndex,
[property: CommandSwitch("--vhub-name")] string VhubName
) : AzOptions;