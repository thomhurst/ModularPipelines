using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network", "firewall", "application-rule", "list")]
public record AzNetworkFirewallApplicationRuleListOptions(
[property: CliOption("--collection-name")] string CollectionName,
[property: CliOption("--firewall-name")] string FirewallName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;