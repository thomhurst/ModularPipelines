using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "vpn-gateway", "connection", "vpn-site-link-conn", "ipsec-policy", "list")]
public record AzNetworkVpnGatewayConnectionVpnSiteLinkConnIpsecPolicyListOptions(
[property: CommandSwitch("--connection-name")] string ConnectionName,
[property: CommandSwitch("--gateway-name")] string GatewayName,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions;