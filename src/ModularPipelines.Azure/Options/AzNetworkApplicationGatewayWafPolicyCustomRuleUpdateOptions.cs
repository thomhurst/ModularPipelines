using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network", "application-gateway", "waf-policy", "custom-rule", "update")]
public record AzNetworkApplicationGatewayWafPolicyCustomRuleUpdateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--policy-name")] string PolicyName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--action")]
    public string? Action { get; set; }

    [CliOption("--add")]
    public string? Add { get; set; }

    [CliFlag("--force-string")]
    public bool? ForceString { get; set; }

    [CliOption("--group-by-user-session")]
    public string? GroupByUserSession { get; set; }

    [CliOption("--match-conditions")]
    public string? MatchConditions { get; set; }

    [CliOption("--priority")]
    public string? Priority { get; set; }

    [CliOption("--rate-limit-duration")]
    public string? RateLimitDuration { get; set; }

    [CliOption("--rate-limit-threshold")]
    public string? RateLimitThreshold { get; set; }

    [CliOption("--remove")]
    public string? Remove { get; set; }

    [CliOption("--rule-type")]
    public string? RuleType { get; set; }

    [CliOption("--set")]
    public string? Set { get; set; }

    [CliOption("--state")]
    public string? State { get; set; }
}