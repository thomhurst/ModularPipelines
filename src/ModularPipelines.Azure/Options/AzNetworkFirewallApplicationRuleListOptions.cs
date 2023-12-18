using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "firewall", "application-rule", "list")]
public record AzNetworkFirewallApplicationRuleListOptions(
[property: CommandSwitch("--collection-name")] string CollectionName,
[property: CommandSwitch("--firewall-name")] string FirewallName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions;