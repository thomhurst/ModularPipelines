using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network", "front-door", "rules-engine", "rule", "show")]
public record AzNetworkFrontDoorRulesEngineRuleShowOptions(
[property: CliOption("--front-door-name")] string FrontDoorName,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--rules-engine-name")] string RulesEngineName
) : AzOptions;