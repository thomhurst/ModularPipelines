using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("network", "vnet-gateway", "vpn-client", "ipsec-policy", "set")]
public record AzNetworkVnetGatewayVpnClientIpsecPolicySetOptions(
[property: CliOption("--dh-group")] string DhGroup,
[property: CliOption("--ike-encryption")] string IkeEncryption,
[property: CliOption("--ike-integrity")] string IkeIntegrity,
[property: CliOption("--ipsec-encryption")] string IpsecEncryption,
[property: CliOption("--ipsec-integrity")] string IpsecIntegrity,
[property: CliOption("--pfs-group")] string PfsGroup,
[property: CliOption("--sa-lifetime")] string SaLifetime,
[property: CliOption("--sa-max-size")] string SaMaxSize
) : AzOptions
{
    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}