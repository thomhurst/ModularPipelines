using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("network", "cross-region-lb", "rule", "list")]
public record AzNetworkCrossRegionLbRuleListOptions(
[property: CliOption("--lb-name")] string LbName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;