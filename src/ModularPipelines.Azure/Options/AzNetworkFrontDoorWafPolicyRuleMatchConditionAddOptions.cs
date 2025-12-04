using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("network", "front-door", "waf-policy", "rule", "match-condition", "add")]
public record AzNetworkFrontDoorWafPolicyRuleMatchConditionAddOptions(
[property: CliOption("--match-variable")] string MatchVariable,
[property: CliOption("--operator")] string Operator,
[property: CliOption("--values")] string Values
) : AzOptions
{
    [CliOption("--defer")]
    public string? Defer { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliFlag("--negate")]
    public bool? Negate { get; set; }

    [CliOption("--policy-name")]
    public string? PolicyName { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--transforms")]
    public string? Transforms { get; set; }
}