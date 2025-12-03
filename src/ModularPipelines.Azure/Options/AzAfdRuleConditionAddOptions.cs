using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("afd", "rule", "condition", "add")]
public record AzAfdRuleConditionAddOptions(
[property: CliOption("--match-variable")] string MatchVariable,
[property: CliOption("--operator")] string Operator
) : AzOptions
{
    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--match-values")]
    public string? MatchValues { get; set; }

    [CliFlag("--negate-condition")]
    public bool? NegateCondition { get; set; }

    [CliOption("--profile-name")]
    public string? ProfileName { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--rule-name")]
    public string? RuleName { get; set; }

    [CliOption("--rule-set-name")]
    public string? RuleSetName { get; set; }

    [CliOption("--selector")]
    public string? Selector { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--transforms")]
    public string? Transforms { get; set; }
}