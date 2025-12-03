using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network", "front-door", "waf-policy", "managed-rules", "exclusion", "list")]
public record AzNetworkFrontDoorWafPolicyManagedRulesExclusionListOptions(
[property: CliOption("--policy-name")] string PolicyName,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--type")] string Type
) : AzOptions
{
    [CliOption("--rule-group-id")]
    public string? RuleGroupId { get; set; }

    [CliOption("--rule-id")]
    public string? RuleId { get; set; }
}