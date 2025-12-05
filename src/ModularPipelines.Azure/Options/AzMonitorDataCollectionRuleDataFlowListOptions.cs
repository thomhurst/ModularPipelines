using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("monitor", "data-collection", "rule", "data-flow", "list")]
public record AzMonitorDataCollectionRuleDataFlowListOptions(
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--rule-name")] string RuleName
) : AzOptions;