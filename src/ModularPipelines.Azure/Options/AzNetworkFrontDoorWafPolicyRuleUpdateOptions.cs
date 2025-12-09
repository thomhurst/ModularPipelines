using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("network", "front-door", "waf-policy", "rule", "update")]
public record AzNetworkFrontDoorWafPolicyRuleUpdateOptions : AzOptions
{
    [CliOption("--action")]
    public string? Action { get; set; }

    [CliOption("--defer")]
    public string? Defer { get; set; }

    [CliFlag("--disabled")]
    public bool? Disabled { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--policy-name")]
    public string? PolicyName { get; set; }

    [CliOption("--priority")]
    public string? Priority { get; set; }

    [CliOption("--rate-limit-duration")]
    public string? RateLimitDuration { get; set; }

    [CliOption("--rate-limit-threshold")]
    public string? RateLimitThreshold { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}