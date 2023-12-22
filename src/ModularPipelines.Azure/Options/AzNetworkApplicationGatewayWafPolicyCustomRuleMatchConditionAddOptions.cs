using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "application-gateway", "waf-policy", "custom-rule", "match-condition", "add")]
public record AzNetworkApplicationGatewayWafPolicyCustomRuleMatchConditionAddOptions(
[property: CommandSwitch("--match-variables")] string MatchVariables,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--operator")] string Operator,
[property: CommandSwitch("--policy-name")] string PolicyName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--index")]
    public string? Index { get; set; }

    [BooleanCommandSwitch("--negate")]
    public bool? Negate { get; set; }

    [CommandSwitch("--transforms")]
    public string? Transforms { get; set; }

    [CommandSwitch("--values")]
    public string? Values { get; set; }
}