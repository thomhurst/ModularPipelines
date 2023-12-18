using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dns-resolver", "forwarding-rule", "update")]
public record AzDnsResolverForwardingRuleUpdateOptions : AzOptions
{
    [CommandSwitch("--forwarding-rule-name")]
    public string? ForwardingRuleName { get; set; }

    [CommandSwitch("--forwarding-rule-state")]
    public string? ForwardingRuleState { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--if-match")]
    public string? IfMatch { get; set; }

    [CommandSwitch("--metadata")]
    public string? Metadata { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--ruleset-name")]
    public string? RulesetName { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }

    [CommandSwitch("--target-dns-servers")]
    public string? TargetDnsServers { get; set; }
}