using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "application-gateway", "waf-policy", "managed-rule", "exclusion", "rule-set", "add")]
public record AzNetworkApplicationGatewayWafPolicyManagedRuleExclusionRuleSetAddOptions(
[property: CommandSwitch("--match-operator")] string MatchOperator,
[property: CommandSwitch("--match-variable")] string MatchVariable,
[property: CommandSwitch("--policy-name")] string PolicyName,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--selector")] string Selector,
[property: CommandSwitch("--type")] string Type,
[property: CommandSwitch("--version")] string Version
) : AzOptions
{
    [CommandSwitch("--group-name")]
    public string? GroupName { get; set; }

    [CommandSwitch("--rule-ids")]
    public string? RuleIds { get; set; }
}