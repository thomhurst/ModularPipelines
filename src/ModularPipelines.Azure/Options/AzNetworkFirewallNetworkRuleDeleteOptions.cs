using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("network", "firewall", "network-rule", "delete")]
public record AzNetworkFirewallNetworkRuleDeleteOptions : AzOptions
{
    [CliOption("--collection-name")]
    public string? CollectionName { get; set; }

    [CliOption("--firewall-name")]
    public string? FirewallName { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}