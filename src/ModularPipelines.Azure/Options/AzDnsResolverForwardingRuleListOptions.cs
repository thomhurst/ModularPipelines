using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dns-resolver", "forwarding-rule", "list")]
public record AzDnsResolverForwardingRuleListOptions(
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--ruleset-name")] string RulesetName
) : AzOptions
{
    [CliOption("--top")]
    public string? Top { get; set; }
}