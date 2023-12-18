using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "front-door", "waf-policy", "rule", "create")]
public record AzNetworkFrontDoorWafPolicyRuleCreateOptions(
[property: CommandSwitch("--action")] string Action,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--policy-name")] string PolicyName,
[property: CommandSwitch("--priority")] string Priority,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--rule-type")] string RuleType
) : AzOptions
{
    [CommandSwitch("--defer")]
    public string? Defer { get; set; }

    [BooleanCommandSwitch("--disabled")]
    public bool? Disabled { get; set; }

    [CommandSwitch("--rate-limit-duration")]
    public string? RateLimitDuration { get; set; }

    [CommandSwitch("--rate-limit-threshold")]
    public string? RateLimitThreshold { get; set; }
}

