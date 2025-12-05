using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("network", "route-filter", "rule", "list-service-communities")]
public record AzNetworkRouteFilterRuleListServiceCommunitiesOptions : AzOptions;