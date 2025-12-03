using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network", "vnet-gateway", "create")]
public record AzNetworkVnetGatewayCreateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--vnet")] string Vnet
) : AzOptions
{
    [CliOption("--aad-audience")]
    public string? AadAudience { get; set; }

    [CliOption("--aad-issuer")]
    public string? AadIssuer { get; set; }

    [CliOption("--aad-tenant")]
    public string? AadTenant { get; set; }

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

    [CliOption("--edge-zone")]
    public string? EdgeZone { get; set; }

    [CliOption("--edge-zone-vnet-id")]
    public string? EdgeZoneVnetId { get; set; }

    [CliOption("--gateway-default-site")]
    public string? GatewayDefaultSite { get; set; }

    [CliOption("--gateway-type")]
    public string? GatewayType { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--nat-rule")]
    public string? NatRule { get; set; }

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

    [CliOption("--root-cert-data")]
    public string? RootCertData { get; set; }

    [CliOption("--root-cert-name")]
    public string? RootCertName { get; set; }

    [CliOption("--sku")]
    public string? Sku { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliOption("--vpn-auth-type")]
    public string? VpnAuthType { get; set; }

    [CliOption("--vpn-gateway-generation")]
    public string? VpnGatewayGeneration { get; set; }

    [CliOption("--vpn-type")]
    public string? VpnType { get; set; }
}