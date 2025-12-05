using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("dns-resolver", "vnet-link", "show")]
public record AzDnsResolverVnetLinkShowOptions : AzOptions
{
    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--ruleset-name")]
    public string? RulesetName { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}