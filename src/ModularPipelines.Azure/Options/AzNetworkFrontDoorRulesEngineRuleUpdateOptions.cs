using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("network", "front-door", "rules-engine", "rule", "update")]
public record AzNetworkFrontDoorRulesEngineRuleUpdateOptions(
[property: CliOption("--front-door-name")] string FrontDoorName,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--rules-engine-name")] string RulesEngineName
) : AzOptions
{
    [CliOption("--match-processing-behavior")]
    public string? MatchProcessingBehavior { get; set; }

    [CliOption("--priority")]
    public string? Priority { get; set; }
}