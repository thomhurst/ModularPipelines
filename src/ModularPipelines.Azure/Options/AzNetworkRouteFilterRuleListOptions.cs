using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network", "route-filter", "rule", "list")]
public record AzNetworkRouteFilterRuleListOptions(
[property: CliOption("--filter-name")] string FilterName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;