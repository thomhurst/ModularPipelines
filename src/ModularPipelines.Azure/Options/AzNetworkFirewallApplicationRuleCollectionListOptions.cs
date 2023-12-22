using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "firewall", "application-rule", "collection", "list")]
public record AzNetworkFirewallApplicationRuleCollectionListOptions(
[property: CommandSwitch("--firewall-name")] string FirewallName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions;