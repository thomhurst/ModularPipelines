using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "front-door", "waf-policy", "managed-rules", "exclusion", "add")]
public record AzNetworkFrontDoorWafPolicyManagedRulesExclusionAddOptions(
[property: CommandSwitch("--match-variable")] string MatchVariable,
[property: CommandSwitch("--operator")] string Operator,
[property: CommandSwitch("--type")] string Type,
[property: CommandSwitch("--value")] string Value
) : AzOptions
{
    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--policy-name")]
    public string? PolicyName { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--rule-group-id")]
    public string? RuleGroupId { get; set; }

    [CommandSwitch("--rule-id")]
    public string? RuleId { get; set; }

    [CommandSwitch("--subscription")]
    public new string? Subscription { get; set; }
}