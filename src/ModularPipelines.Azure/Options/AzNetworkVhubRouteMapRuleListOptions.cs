using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("network", "vhub", "route-map", "rule", "list")]
public record AzNetworkVhubRouteMapRuleListOptions(
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--route-map-name")] string RouteMapName,
[property: CliOption("--vhub-name")] string VhubName
) : AzOptions;