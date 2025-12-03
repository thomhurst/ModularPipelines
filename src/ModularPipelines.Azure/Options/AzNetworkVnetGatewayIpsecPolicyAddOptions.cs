using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network", "vnet-gateway", "ipsec-policy", "add")]
public record AzNetworkVnetGatewayIpsecPolicyAddOptions(
[property: CliOption("--dh-group")] string DhGroup,
[property: CliOption("--gateway-name")] string GatewayName,
[property: CliOption("--ike-encryption")] string IkeEncryption,
[property: CliOption("--ike-integrity")] string IkeIntegrity,
[property: CliOption("--ipsec-encryption")] string IpsecEncryption,
[property: CliOption("--ipsec-integrity")] string IpsecIntegrity,
[property: CliOption("--pfs-group")] string PfsGroup,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--sa-lifetime")] string SaLifetime,
[property: CliOption("--sa-max-size")] string SaMaxSize
) : AzOptions
{
    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }
}