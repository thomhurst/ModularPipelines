using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cdn", "waf", "policy", "rate-limit-rule", "set")]
public record AzCdnWafPolicyRateLimitRuleSetOptions(
[property: CommandSwitch("--action")] string Action,
[property: CommandSwitch("--duration")] string Duration,
[property: CommandSwitch("--match-condition")] string MatchCondition,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--priority")] string Priority,
[property: CommandSwitch("--request-threshold")] string RequestThreshold
) : AzOptions
{
    [BooleanCommandSwitch("--disabled")]
    public bool? Disabled { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--policy-name")]
    public string? PolicyName { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }
}