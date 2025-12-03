using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network", "route-filter", "rule", "list-service-communities")]
public record AzNetworkRouteFilterRuleListServiceCommunitiesOptions : AzOptions;