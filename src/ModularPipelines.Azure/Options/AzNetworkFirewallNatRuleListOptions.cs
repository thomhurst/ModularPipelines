using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("network", "firewall", "nat-rule", "list")]
public record AzNetworkFirewallNatRuleListOptions(
[property: CliOption("--collection-name")] string CollectionName,
[property: CliOption("--firewall-name")] string FirewallName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;