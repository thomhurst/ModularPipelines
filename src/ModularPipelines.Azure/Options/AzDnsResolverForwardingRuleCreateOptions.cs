using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("dns-resolver", "forwarding-rule", "create")]
public record AzDnsResolverForwardingRuleCreateOptions(
[property: CliOption("--domain-name")] string DomainName,
[property: CliOption("--forwarding-rule-name")] string ForwardingRuleName,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--ruleset-name")] string RulesetName,
[property: CliOption("--target-dns-servers")] string TargetDnsServers
) : AzOptions
{
    [CliOption("--forwarding-rule-state")]
    public string? ForwardingRuleState { get; set; }

    [CliOption("--if-match")]
    public string? IfMatch { get; set; }

    [CliOption("--if-none-match")]
    public string? IfNoneMatch { get; set; }

    [CliOption("--metadata")]
    public string? Metadata { get; set; }
}