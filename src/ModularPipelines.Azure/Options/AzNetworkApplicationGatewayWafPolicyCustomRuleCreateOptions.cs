using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("network", "application-gateway", "waf-policy", "custom-rule", "create")]
public record AzNetworkApplicationGatewayWafPolicyCustomRuleCreateOptions(
[property: CliOption("--action")] string Action,
[property: CliOption("--name")] string Name,
[property: CliOption("--policy-name")] string PolicyName,
[property: CliOption("--priority")] string Priority,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--rule-type")] string RuleType
) : AzOptions
{
    [CliOption("--group-by-user-session")]
    public string? GroupByUserSession { get; set; }

    [CliOption("--match-conditions")]
    public string? MatchConditions { get; set; }

    [CliOption("--rate-limit-duration")]
    public string? RateLimitDuration { get; set; }

    [CliOption("--rate-limit-threshold")]
    public string? RateLimitThreshold { get; set; }

    [CliOption("--state")]
    public string? State { get; set; }
}