using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "route-filter", "rule", "list")]
public record AzNetworkRouteFilterRuleListOptions(
[property: CommandSwitch("--filter-name")] string FilterName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions;