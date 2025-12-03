using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network", "front-door", "waf-policy", "rule", "match-condition", "list")]
public record AzNetworkFrontDoorWafPolicyRuleMatchConditionListOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--policy-name")] string PolicyName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--defer")]
    public string? Defer { get; set; }
}