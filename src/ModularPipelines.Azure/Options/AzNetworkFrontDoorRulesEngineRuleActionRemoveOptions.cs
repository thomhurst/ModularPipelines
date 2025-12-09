using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("network", "front-door", "rules-engine", "rule", "action", "remove")]
public record AzNetworkFrontDoorRulesEngineRuleActionRemoveOptions(
[property: CliOption("--action-type")] string ActionType,
[property: CliOption("--front-door-name")] string FrontDoorName,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--rules-engine-name")] string RulesEngineName
) : AzOptions
{
    [CliOption("--index")]
    public string? Index { get; set; }
}