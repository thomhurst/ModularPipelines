using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "application-gateway", "waf-policy")]
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