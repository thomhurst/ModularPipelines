using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "front-door", "rules-engine", "rule", "action", "remove")]
public record AzNetworkFrontDoorRulesEngineRuleActionRemoveOptions(
[property: CommandSwitch("--action-type")] string ActionType,
[property: CommandSwitch("--front-door-name")] string FrontDoorName,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--rules-engine-name")] string RulesEngineName
) : AzOptions
{
    [CommandSwitch("--index")]
    public string? Index { get; set; }
}