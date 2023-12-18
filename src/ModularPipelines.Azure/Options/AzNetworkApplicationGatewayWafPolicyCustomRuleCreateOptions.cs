using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "application-gateway", "waf-policy", "custom-rule", "create")]
public record AzNetworkApplicationGatewayWafPolicyCustomRuleCreateOptions(
[property: CommandSwitch("--action")] string Action,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--policy-name")] string PolicyName,
[property: CommandSwitch("--priority")] string Priority,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--rule-type")] string RuleType
) : AzOptions
{
    [CommandSwitch("--group-by-user-session")]
    public string? GroupByUserSession { get; set; }

    [CommandSwitch("--match-conditions")]
    public string? MatchConditions { get; set; }

    [CommandSwitch("--rate-limit-duration")]
    public string? RateLimitDuration { get; set; }

    [CommandSwitch("--rate-limit-threshold")]
    public string? RateLimitThreshold { get; set; }

    [CommandSwitch("--state")]
    public string? State { get; set; }
}