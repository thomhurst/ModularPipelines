using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "vnet-gateway", "vpn-client", "ipsec-policy", "set")]
public record AzNetworkVnetGatewayVpnClientIpsecPolicySetOptions(
[property: CommandSwitch("--dh-group")] string DhGroup,
[property: CommandSwitch("--ike-encryption")] string IkeEncryption,
[property: CommandSwitch("--ike-integrity")] string IkeIntegrity,
[property: CommandSwitch("--ipsec-encryption")] string IpsecEncryption,
[property: CommandSwitch("--ipsec-integrity")] string IpsecIntegrity,
[property: CommandSwitch("--pfs-group")] string PfsGroup,
[property: CommandSwitch("--sa-lifetime")] string SaLifetime,
[property: CommandSwitch("--sa-max-size")] string SaMaxSize
) : AzOptions
{
    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }
}

