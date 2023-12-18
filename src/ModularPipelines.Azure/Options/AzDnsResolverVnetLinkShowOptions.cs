using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dns-resolver", "vnet-link", "show")]
public record AzDnsResolverVnetLinkShowOptions : AzOptions
{
    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--ruleset-name")]
    public string? RulesetName { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }
}