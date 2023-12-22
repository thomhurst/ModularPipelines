using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "firewall", "ip-config", "list")]
public record AzNetworkFirewallIpConfigListOptions(
[property: CommandSwitch("--firewall-name")] string FirewallName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions;