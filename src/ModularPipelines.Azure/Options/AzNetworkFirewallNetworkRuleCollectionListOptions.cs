using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("network", "firewall", "network-rule", "collection", "list")]
public record AzNetworkFirewallNetworkRuleCollectionListOptions(
[property: CliOption("--firewall-name")] string FirewallName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;