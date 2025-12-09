using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("network", "application-gateway", "waf-policy")]
public class AzNetworkApplicationGatewayWafPolicyManagedRule
{
    public AzNetworkApplicationGatewayWafPolicyManagedRule(
        AzNetworkApplicationGatewayWafPolicyManagedRuleExclusion exclusion,
        AzNetworkApplicationGatewayWafPolicyManagedRuleRuleSet ruleSet
    )
    {
        Exclusion = exclusion;
        RuleSet = ruleSet;
    }

    public AzNetworkApplicationGatewayWafPolicyManagedRuleExclusion Exclusion { get; }

    public AzNetworkApplicationGatewayWafPolicyManagedRuleRuleSet RuleSet { get; }
}