using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "vpn-gateway", "connection", "vpn-site-link-conn", "add")]
public record AzNetworkVpnGatewayConnectionVpnSiteLinkConnAddOptions(
[property: CommandSwitch("--vpn-site-link")] string VpnSiteLink
) : AzOptions
{
    [CommandSwitch("--connection-bandwidth")]
    public string? ConnectionBandwidth { get; set; }

    [CommandSwitch("--connection-name")]
    public string? ConnectionName { get; set; }

    [BooleanCommandSwitch("--enable-bgp")]
    public bool? EnableBgp { get; set; }

    [CommandSwitch("--gateway-name")]
    public string? GatewayName { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [BooleanCommandSwitch("--rate-limiting")]
    public bool? RateLimiting { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--routing-weight")]
    public string? RoutingWeight { get; set; }

    [CommandSwitch("--shared-key")]
    public string? SharedKey { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }

    [BooleanCommandSwitch("--use-local-azure-ip-address")]
    public bool? UseLocalAzureIpAddress { get; set; }

    [BooleanCommandSwitch("--use-policy-based-traffic-selectors")]
    public bool? UsePolicyBasedTrafficSelectors { get; set; }

    [CommandSwitch("--vpn-connection-protocol-type")]
    public string? VpnConnectionProtocolType { get; set; }

    [CommandSwitch("--vpn-link-connection-mode")]
    public string? VpnLinkConnectionMode { get; set; }
}

