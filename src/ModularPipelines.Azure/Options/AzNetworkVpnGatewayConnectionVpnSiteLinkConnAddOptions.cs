using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network", "vpn-gateway", "connection", "vpn-site-link-conn", "add")]
public record AzNetworkVpnGatewayConnectionVpnSiteLinkConnAddOptions(
[property: CliOption("--vpn-site-link")] string VpnSiteLink
) : AzOptions
{
    [CliOption("--connection-bandwidth")]
    public string? ConnectionBandwidth { get; set; }

    [CliOption("--connection-name")]
    public string? ConnectionName { get; set; }

    [CliFlag("--enable-bgp")]
    public bool? EnableBgp { get; set; }

    [CliOption("--gateway-name")]
    public string? GatewayName { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliFlag("--rate-limiting")]
    public bool? RateLimiting { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--routing-weight")]
    public string? RoutingWeight { get; set; }

    [CliOption("--shared-key")]
    public string? SharedKey { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliFlag("--use-local-azure-ip-address")]
    public bool? UseLocalAzureIpAddress { get; set; }

    [CliFlag("--use-policy-based-traffic-selectors")]
    public bool? UsePolicyBasedTrafficSelectors { get; set; }

    [CliOption("--vpn-connection-protocol-type")]
    public string? VpnConnectionProtocolType { get; set; }

    [CliOption("--vpn-link-connection-mode")]
    public string? VpnLinkConnectionMode { get; set; }
}