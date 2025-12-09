using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("network", "vpn-gateway", "connection", "ipsec-policy", "list")]
public record AzNetworkVpnGatewayConnectionIpsecPolicyListOptions(
[property: CliOption("--connection-name")] string ConnectionName,
[property: CliOption("--gateway-name")] string GatewayName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;