using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "vpn-gateway", "connection", "ipsec-policy", "remove")]
public record AzNetworkVpnGatewayConnectionIpsecPolicyRemoveOptions(
[property: CommandSwitch("--connection-name")] string ConnectionName,
[property: CommandSwitch("--gateway-name")] string GatewayName,
[property: CommandSwitch("--index")] string Index,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }
}