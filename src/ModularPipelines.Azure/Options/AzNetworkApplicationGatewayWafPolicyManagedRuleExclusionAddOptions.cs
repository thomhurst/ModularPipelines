using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network", "application-gateway", "waf-policy", "managed-rule", "exclusion", "add")]
public record AzNetworkApplicationGatewayWafPolicyManagedRuleExclusionAddOptions(
[property: CliOption("--match-operator")] string MatchOperator,
[property: CliOption("--match-variable")] string MatchVariable,
[property: CliOption("--policy-name")] string PolicyName,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--selector")] string Selector
) : AzOptions
{
    [CliOption("--index")]
    public string? Index { get; set; }

    [CliOption("--rule-sets")]
    public string? RuleSets { get; set; }
}