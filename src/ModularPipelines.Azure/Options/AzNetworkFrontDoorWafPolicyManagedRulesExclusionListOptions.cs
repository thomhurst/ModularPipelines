using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "front-door", "waf-policy", "managed-rules", "exclusion", "list")]
public record AzNetworkFrontDoorWafPolicyManagedRulesExclusionListOptions(
[property: CommandSwitch("--policy-name")] string PolicyName,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--type")] string Type
) : AzOptions
{
    [CommandSwitch("--rule-group-id")]
    public string? RuleGroupId { get; set; }

    [CommandSwitch("--rule-id")]
    public string? RuleId { get; set; }
}