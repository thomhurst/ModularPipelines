using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dns-resolver", "forwarding-ruleset", "show")]
public record AzDnsResolverForwardingRulesetShowOptions : AzOptions
{
    [CliOption("--dns-forwarding-ruleset-name")]
    public string? DnsForwardingRulesetName { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}