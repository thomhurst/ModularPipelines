using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "vpn-gateway", "connection", "vpn-site-link-conn", "ipsec-policy", "add")]
public record AzNetworkVpnGatewayConnectionVpnSiteLinkConnIpsecPolicyAddOptions(
[property: CommandSwitch("--dh-group")] string DhGroup,
[property: CommandSwitch("--ike-encryption")] string IkeEncryption,
[property: CommandSwitch("--ike-integrity")] string IkeIntegrity,
[property: CommandSwitch("--ipsec-encryption")] string IpsecEncryption,
[property: CommandSwitch("--ipsec-integrity")] string IpsecIntegrity,
[property: CommandSwitch("--pfs-group")] string PfsGroup,
[property: CommandSwitch("--sa-data-size")] string SaDataSize,
[property: CommandSwitch("--sa-lifetime")] string SaLifetime
) : AzOptions
{
    [CommandSwitch("--connection-name")]
    public string? ConnectionName { get; set; }

    [CommandSwitch("--gateway-name")]
    public string? GatewayName { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--subscription")]
    public new string? Subscription { get; set; }
}