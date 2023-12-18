using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "application-gateway", "rewrite-rule", "condition", "delete")]
public record AzNetworkApplicationGatewayRewriteRuleConditionDeleteOptions(
[property: CommandSwitch("--gateway-name")] string GatewayName,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--rule-name")] string RuleName,
[property: CommandSwitch("--rule-set-name")] string RuleSetName,
[property: CommandSwitch("--variable")] string Variable
) : AzOptions
{
    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }
}