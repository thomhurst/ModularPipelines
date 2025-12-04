using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("dns-resolver", "vnet-link", "list")]
public record AzDnsResolverVnetLinkListOptions(
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--ruleset-name")] string RulesetName
) : AzOptions
{
    [CliOption("--top")]
    public string? Top { get; set; }
}