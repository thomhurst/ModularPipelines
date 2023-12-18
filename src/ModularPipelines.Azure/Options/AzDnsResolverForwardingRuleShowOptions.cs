using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dns-resolver", "forwarding-rule", "show")]
public record AzDnsResolverForwardingRuleShowOptions : AzOptions
{
    [CommandSwitch("--forwarding-rule-name")]
    public string? ForwardingRuleName { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--ruleset-name")]
    public string? RulesetName { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }
}

