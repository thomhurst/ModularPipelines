using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("afd", "rule", "condition", "add")]
public record AzAfdRuleConditionAddOptions(
[property: CommandSwitch("--match-variable")] string MatchVariable,
[property: CommandSwitch("--operator")] string Operator
) : AzOptions
{
    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--match-values")]
    public string? MatchValues { get; set; }

    [BooleanCommandSwitch("--negate-condition")]
    public bool? NegateCondition { get; set; }

    [CommandSwitch("--profile-name")]
    public string? ProfileName { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--rule-name")]
    public string? RuleName { get; set; }

    [CommandSwitch("--rule-set-name")]
    public string? RuleSetName { get; set; }

    [CommandSwitch("--selector")]
    public string? Selector { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }

    [CommandSwitch("--transforms")]
    public string? Transforms { get; set; }
}

