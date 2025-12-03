using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cdn", "endpoint", "rule", "condition", "add")]
public record AzCdnEndpointRuleConditionAddOptions(
[property: CliOption("--match-variable")] string MatchVariable,
[property: CliOption("--operator")] string Operator,
[property: CliOption("--rule-name")] string RuleName
) : AzOptions
{
    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--match-values")]
    public string? MatchValues { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliFlag("--negate-condition")]
    public bool? NegateCondition { get; set; }

    [CliOption("--profile-name")]
    public string? ProfileName { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--selector")]
    public string? Selector { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--transform")]
    public string? Transform { get; set; }
}