using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network", "firewall", "ip-config", "list")]
public record AzNetworkFirewallIpConfigListOptions(
[property: CliOption("--firewall-name")] string FirewallName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;