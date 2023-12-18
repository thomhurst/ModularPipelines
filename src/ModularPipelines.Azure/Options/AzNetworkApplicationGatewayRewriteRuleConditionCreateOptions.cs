using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "application-gateway", "rewrite-rule", "condition", "create")]
public record AzNetworkApplicationGatewayRewriteRuleConditionCreateOptions(
[property: CommandSwitch("--gateway-name")] string GatewayName,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--rule-name")] string RuleName,
[property: CommandSwitch("--rule-set-name")] string RuleSetName,
[property: CommandSwitch("--variable")] string Variable
) : AzOptions
{
    [BooleanCommandSwitch("--ignore-case")]
    public bool? IgnoreCase { get; set; }

    [BooleanCommandSwitch("--negate")]
    public bool? Negate { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--pattern")]
    public string? Pattern { get; set; }
}