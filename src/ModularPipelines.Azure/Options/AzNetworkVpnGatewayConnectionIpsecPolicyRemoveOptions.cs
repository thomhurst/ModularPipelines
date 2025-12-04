using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("network", "vpn-gateway", "connection", "ipsec-policy", "remove")]
public record AzNetworkVpnGatewayConnectionIpsecPolicyRemoveOptions(
[property: CliOption("--connection-name")] string ConnectionName,
[property: CliOption("--gateway-name")] string GatewayName,
[property: CliOption("--index")] string Index,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }
}