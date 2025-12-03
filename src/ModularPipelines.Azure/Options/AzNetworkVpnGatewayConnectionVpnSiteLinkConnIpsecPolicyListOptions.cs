using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network", "vpn-gateway", "connection", "vpn-site-link-conn", "ipsec-policy", "list")]
public record AzNetworkVpnGatewayConnectionVpnSiteLinkConnIpsecPolicyListOptions(
[property: CliOption("--connection-name")] string ConnectionName,
[property: CliOption("--gateway-name")] string GatewayName,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;