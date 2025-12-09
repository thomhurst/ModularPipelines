using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("network", "front-door", "waf-policy", "managed-rules", "exclusion", "add")]
public record AzNetworkFrontDoorWafPolicyManagedRulesExclusionAddOptions(
[property: CliOption("--match-variable")] string MatchVariable,
[property: CliOption("--operator")] string Operator,
[property: CliOption("--type")] string Type,
[property: CliOption("--value")] string Value
) : AzOptions
{
    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--policy-name")]
    public string? PolicyName { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--rule-group-id")]
    public string? RuleGroupId { get; set; }

    [CliOption("--rule-id")]
    public string? RuleId { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}