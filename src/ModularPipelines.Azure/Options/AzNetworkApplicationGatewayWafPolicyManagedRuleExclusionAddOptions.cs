using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "application-gateway", "waf-policy", "managed-rule", "exclusion", "add")]
public record AzNetworkApplicationGatewayWafPolicyManagedRuleExclusionAddOptions(
[property: CommandSwitch("--match-operator")] string MatchOperator,
[property: CommandSwitch("--match-variable")] string MatchVariable,
[property: CommandSwitch("--policy-name")] string PolicyName,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--selector")] string Selector
) : AzOptions
{
    [CommandSwitch("--index")]
    public string? Index { get; set; }

    [CommandSwitch("--rule-sets")]
    public string? RuleSets { get; set; }
}