using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("network", "front-door", "routing-rule", "list")]
public record AzNetworkFrontDoorRoutingRuleListOptions(
[property: CliOption("--front-door-name")] string FrontDoorName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;