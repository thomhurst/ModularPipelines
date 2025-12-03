using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dns-resolver", "forwarding-rule", "update")]
public record AzDnsResolverForwardingRuleUpdateOptions : AzOptions
{
    [CliOption("--forwarding-rule-name")]
    public string? ForwardingRuleName { get; set; }

    [CliOption("--forwarding-rule-state")]
    public string? ForwardingRuleState { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--if-match")]
    public string? IfMatch { get; set; }

    [CliOption("--metadata")]
    public string? Metadata { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--ruleset-name")]
    public string? RulesetName { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--target-dns-servers")]
    public string? TargetDnsServers { get; set; }
}