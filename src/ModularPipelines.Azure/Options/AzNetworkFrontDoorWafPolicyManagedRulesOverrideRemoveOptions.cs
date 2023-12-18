using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "front-door", "waf-policy", "managed-rules", "override", "remove")]
public record AzNetworkFrontDoorWafPolicyManagedRulesOverrideRemoveOptions(
[property: CommandSwitch("--rule-group-id")] string RuleGroupId,
[property: CommandSwitch("--rule-id")] string RuleId,
[property: CommandSwitch("--type")] string Type
) : AzOptions
{
    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--policy-name")]
    public string? PolicyName { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }
}