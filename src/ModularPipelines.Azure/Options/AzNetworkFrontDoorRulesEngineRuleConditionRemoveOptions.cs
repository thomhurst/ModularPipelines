using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("network", "front-door", "rules-engine", "rule", "condition", "remove")]
public record AzNetworkFrontDoorRulesEngineRuleConditionRemoveOptions(
[property: CliOption("--front-door-name")] string FrontDoorName,
[property: CliOption("--index")] string Index,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--rules-engine-name")] string RulesEngineName
) : AzOptions;