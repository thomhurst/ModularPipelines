using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("network", "application-gateway", "waf-policy", "managed-rule", "exclusion", "rule-set", "add")]
public record AzNetworkApplicationGatewayWafPolicyManagedRuleExclusionRuleSetAddOptions(
[property: CliOption("--match-operator")] string MatchOperator,
[property: CliOption("--match-variable")] string MatchVariable,
[property: CliOption("--policy-name")] string PolicyName,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--selector")] string Selector,
[property: CliOption("--type")] string Type,
[property: CliOption("--version")] string Version
) : AzOptions
{
    [CliOption("--group-name")]
    public string? GroupName { get; set; }

    [CliOption("--rule-ids")]
    public string? RuleIds { get; set; }
}