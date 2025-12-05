using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("network", "lb", "outbound-rule", "list")]
public record AzNetworkLbOutboundRuleListOptions(
[property: CliOption("--lb-name")] string LbName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;