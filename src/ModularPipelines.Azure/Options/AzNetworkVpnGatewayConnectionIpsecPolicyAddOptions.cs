using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "vpn-gateway", "connection", "ipsec-policy", "add")]
public record AzNetworkVpnGatewayConnectionIpsecPolicyAddOptions(
[property: CommandSwitch("--connection-name")] string ConnectionName,
[property: CommandSwitch("--dh-group")] string DhGroup,
[property: CommandSwitch("--gateway-name")] string GatewayName,
[property: CommandSwitch("--ike-encryption")] string IkeEncryption,
[property: CommandSwitch("--ike-integrity")] string IkeIntegrity,
[property: CommandSwitch("--ipsec-encryption")] string IpsecEncryption,
[property: CommandSwitch("--ipsec-integrity")] string IpsecIntegrity,
[property: CommandSwitch("--pfs-group")] string PfsGroup,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--sa-data-size")] string SaDataSize,
[property: CommandSwitch("--sa-lifetime")] string SaLifetime
) : AzOptions
{
    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }
}