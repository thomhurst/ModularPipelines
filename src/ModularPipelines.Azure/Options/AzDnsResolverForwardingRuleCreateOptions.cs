using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dns-resolver", "forwarding-rule", "create")]
public record AzDnsResolverForwardingRuleCreateOptions(
[property: CommandSwitch("--domain-name")] string DomainName,
[property: CommandSwitch("--forwarding-rule-name")] string ForwardingRuleName,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--ruleset-name")] string RulesetName,
[property: CommandSwitch("--target-dns-servers")] string TargetDnsServers
) : AzOptions
{
    [CommandSwitch("--forwarding-rule-state")]
    public string? ForwardingRuleState { get; set; }

    [CommandSwitch("--if-match")]
    public string? IfMatch { get; set; }

    [CommandSwitch("--if-none-match")]
    public string? IfNoneMatch { get; set; }

    [CommandSwitch("--metadata")]
    public string? Metadata { get; set; }
}

