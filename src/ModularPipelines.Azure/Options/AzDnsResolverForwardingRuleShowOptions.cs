using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("dns-resolver", "forwarding-rule", "show")]
public record AzDnsResolverForwardingRuleShowOptions : AzOptions
{
    [CliOption("--forwarding-rule-name")]
    public string? ForwardingRuleName { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--ruleset-name")]
    public string? RulesetName { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}