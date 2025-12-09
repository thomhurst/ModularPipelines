using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("network", "vpn-gateway", "connection", "ipsec-policy", "add")]
public record AzNetworkVpnGatewayConnectionIpsecPolicyAddOptions(
[property: CliOption("--connection-name")] string ConnectionName,
[property: CliOption("--dh-group")] string DhGroup,
[property: CliOption("--gateway-name")] string GatewayName,
[property: CliOption("--ike-encryption")] string IkeEncryption,
[property: CliOption("--ike-integrity")] string IkeIntegrity,
[property: CliOption("--ipsec-encryption")] string IpsecEncryption,
[property: CliOption("--ipsec-integrity")] string IpsecIntegrity,
[property: CliOption("--pfs-group")] string PfsGroup,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--sa-data-size")] string SaDataSize,
[property: CliOption("--sa-lifetime")] string SaLifetime
) : AzOptions
{
    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }
}