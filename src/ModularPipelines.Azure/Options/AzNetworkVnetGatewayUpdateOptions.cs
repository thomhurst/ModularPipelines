using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "vnet-gateway", "update")]
public record AzNetworkVnetGatewayUpdateOptions : AzOptions
{
    [CommandSwitch("--aad-audience")]
    public string? AadAudience { get; set; }

    [CommandSwitch("--aad-issuer")]
    public string? AadIssuer { get; set; }

    [CommandSwitch("--aad-tenant")]
    public string? AadTenant { get; set; }

    [CommandSwitch("--add")]
    public string? Add { get; set; }

    [CommandSwitch("--address-prefix")]
    public string? AddressPrefix { get; set; }

    [CommandSwitch("--asn")]
    public string? Asn { get; set; }

    [CommandSwitch("--bgp-peering-address")]
    public string? BgpPeeringAddress { get; set; }

    [CommandSwitch("--client-protocol")]
    public string? ClientProtocol { get; set; }

    [CommandSwitch("--custom-routes")]
    public string? CustomRoutes { get; set; }

    [BooleanCommandSwitch("--enable-bgp")]
    public bool? EnableBgp { get; set; }

    [BooleanCommandSwitch("--force-string")]
    public bool? ForceString { get; set; }

    [CommandSwitch("--gateway-default-site")]
    public string? GatewayDefaultSite { get; set; }

    [CommandSwitch("--gateway-type")]
    public string? GatewayType { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--peer-weight")]
    public string? PeerWeight { get; set; }

    [CommandSwitch("--public-ip-address")]
    public string? PublicIpAddress { get; set; }

    [CommandSwitch("--radius-secret")]
    public string? RadiusSecret { get; set; }

    [CommandSwitch("--radius-server")]
    public string? RadiusServer { get; set; }

    [CommandSwitch("--remove")]
    public string? Remove { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--root-cert-data")]
    public string? RootCertData { get; set; }

    [CommandSwitch("--root-cert-name")]
    public string? RootCertName { get; set; }

    [CommandSwitch("--set")]
    public string? Set { get; set; }

    [CommandSwitch("--sku")]
    public string? Sku { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }

    [CommandSwitch("--vnet")]
    public string? Vnet { get; set; }

    [CommandSwitch("--vpn-auth-type")]
    public string? VpnAuthType { get; set; }

    [CommandSwitch("--vpn-type")]
    public string? VpnType { get; set; }
}