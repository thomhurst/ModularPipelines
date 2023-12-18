using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "front-door", "waf-policy", "rule", "match-condition", "add")]
public record AzNetworkFrontDoorWafPolicyRuleMatchConditionAddOptions(
[property: CommandSwitch("--match-variable")] string MatchVariable,
[property: CommandSwitch("--operator")] string Operator,
[property: CommandSwitch("--values")] string Values
) : AzOptions
{
    [CommandSwitch("--defer")]
    public string? Defer { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [BooleanCommandSwitch("--negate")]
    public bool? Negate { get; set; }

    [CommandSwitch("--policy-name")]
    public string? PolicyName { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }

    [CommandSwitch("--transforms")]
    public string? Transforms { get; set; }
}

