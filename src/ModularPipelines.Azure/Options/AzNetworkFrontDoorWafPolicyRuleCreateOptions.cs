using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("network", "front-door", "waf-policy", "rule", "create")]
public record AzNetworkFrontDoorWafPolicyRuleCreateOptions(
[property: CliOption("--action")] string Action,
[property: CliOption("--name")] string Name,
[property: CliOption("--policy-name")] string PolicyName,
[property: CliOption("--priority")] string Priority,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--rule-type")] string RuleType
) : AzOptions
{
    [CliOption("--defer")]
    public string? Defer { get; set; }

    [CliFlag("--disabled")]
    public bool? Disabled { get; set; }

    [CliOption("--rate-limit-duration")]
    public string? RateLimitDuration { get; set; }

    [CliOption("--rate-limit-threshold")]
    public string? RateLimitThreshold { get; set; }
}