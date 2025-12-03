using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network", "vhub", "route-map", "rule", "add")]
public record AzNetworkVhubRouteMapRuleAddOptions(
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--route-map-name")] string RouteMapName,
[property: CliOption("--vhub-name")] string VhubName
) : AzOptions
{
    [CliOption("--actions")]
    public string? Actions { get; set; }

    [CliOption("--match-criteria")]
    public string? MatchCriteria { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--next-step")]
    public string? NextStep { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--rule-index")]
    public string? RuleIndex { get; set; }
}