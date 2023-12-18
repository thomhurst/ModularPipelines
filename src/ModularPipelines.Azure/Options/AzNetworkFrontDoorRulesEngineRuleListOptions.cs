using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "front-door", "rules-engine", "rule", "list")]
public record AzNetworkFrontDoorRulesEngineRuleListOptions(
[property: CommandSwitch("--front-door-name")] string FrontDoorName,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--match-processing-behavior")]
    public string? MatchProcessingBehavior { get; set; }

    [CommandSwitch("--priority")]
    public string? Priority { get; set; }
}

