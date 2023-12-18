using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "front-door", "rules-engine", "rule", "condition", "add")]
public record AzNetworkFrontDoorRulesEngineRuleConditionAddOptions(
[property: CommandSwitch("--front-door-name")] string FrontDoorName,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--rules-engine-name")] string RulesEngineName
) : AzOptions
{
    [CommandSwitch("--match-values")]
    public string? MatchValues { get; set; }

    [CommandSwitch("--match-variable")]
    public string? MatchVariable { get; set; }

    [BooleanCommandSwitch("--negate-condition")]
    public bool? NegateCondition { get; set; }

    [CommandSwitch("--operator")]
    public string? Operator { get; set; }

    [CommandSwitch("--selector")]
    public string? Selector { get; set; }

    [CommandSwitch("--transforms")]
    public string? Transforms { get; set; }
}