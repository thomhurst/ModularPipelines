using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("network", "firewall", "nat-rule", "collection", "list")]
public record AzNetworkFirewallNatRuleCollectionListOptions(
[property: CliOption("--firewall-name")] string FirewallName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;