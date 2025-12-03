using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network", "vnet-gateway", "update")]
public record AzNetworkVnetGatewayUpdateOptions : AzOptions
{
    [CliOption("--aad-audience")]
    public string? AadAudience { get; set; }

    [CliOption("--aad-issuer")]
    public string? AadIssuer { get; set; }

    [CliOption("--aad-tenant")]
    public string? AadTenant { get; set; }

    [CliOption("--add")]
    public string? Add { get; set; }

    [CliOption("--address-prefix")]
    public string? AddressPrefix { get; set; }

    [CliOption("--asn")]
    public string? Asn { get; set; }

    [CliOption("--bgp-peering-address")]
    public string? BgpPeeringAddress { get; set; }

    [CliOption("--client-protocol")]
    public string? ClientProtocol { get; set; }

    [CliOption("--custom-routes")]
    public string? CustomRoutes { get; set; }

    [CliFlag("--enable-bgp")]
    public bool? EnableBgp { get; set; }

    [CliFlag("--force-string")]
    public bool? ForceString { get; set; }

    [CliOption("--gateway-default-site")]
    public string? GatewayDefaultSite { get; set; }

    [CliOption("--gateway-type")]
    public string? GatewayType { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--peer-weight")]
    public string? PeerWeight { get; set; }

    [CliOption("--public-ip-address")]
    public string? PublicIpAddress { get; set; }

    [CliOption("--radius-secret")]
    public string? RadiusSecret { get; set; }

    [CliOption("--radius-server")]
    public string? RadiusServer { get; set; }

    [CliOption("--remove")]
    public string? Remove { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--root-cert-data")]
    public string? RootCertData { get; set; }

    [CliOption("--root-cert-name")]
    public string? RootCertName { get; set; }

    [CliOption("--set")]
    public string? Set { get; set; }

    [CliOption("--sku")]
    public string? Sku { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliOption("--vnet")]
    public string? Vnet { get; set; }

    [CliOption("--vpn-auth-type")]
    public string? VpnAuthType { get; set; }

    [CliOption("--vpn-type")]
    public string? VpnType { get; set; }
}