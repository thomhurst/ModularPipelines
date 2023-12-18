using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "vpn-server-config", "ipsec-policy", "add")]
public record AzNetworkVpnServerConfigIpsecPolicyAddOptions(
[property: CommandSwitch("--dh-group")] string DhGroup,
[property: CommandSwitch("--ike-encryption")] string IkeEncryption,
[property: CommandSwitch("--ike-integrity")] string IkeIntegrity,
[property: CommandSwitch("--ipsec-encryption")] string IpsecEncryption,
[property: CommandSwitch("--ipsec-integrity")] string IpsecIntegrity,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--pfs-group")] string PfsGroup,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--sa-data-size")] string SaDataSize,
[property: CommandSwitch("--sa-lifetime")] string SaLifetime
) : AzOptions
{
    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }
}

