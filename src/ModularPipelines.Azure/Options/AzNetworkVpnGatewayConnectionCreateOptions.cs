using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network", "vpn-gateway", "connection", "create")]
public record AzNetworkVpnGatewayConnectionCreateOptions(
[property: CliOption("--gateway-name")] string GatewayName,
[property: CliOption("--name")] string Name,
[property: CliOption("--remote-vpn-site")] string RemoteVpnSite,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--associated")]
    public string? Associated { get; set; }

    [CliOption("--associated-inbound-routemap")]
    public string? AssociatedInboundRoutemap { get; set; }

    [CliOption("--associated-outbound-routemap")]
    public string? AssociatedOutboundRoutemap { get; set; }

    [CliOption("--connection-bandwidth")]
    public string? ConnectionBandwidth { get; set; }

    [CliFlag("--enable-bgp")]
    public bool? EnableBgp { get; set; }

    [CliFlag("--internet-security")]
    public bool? InternetSecurity { get; set; }

    [CliOption("--labels")]
    public string? Labels { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--propagated")]
    public string? Propagated { get; set; }

    [CliOption("--protocol-type")]
    public string? ProtocolType { get; set; }

    [CliFlag("--rate-limiting")]
    public bool? RateLimiting { get; set; }

    [CliOption("--routing-weight")]
    public string? RoutingWeight { get; set; }

    [CliOption("--shared-key")]
    public string? SharedKey { get; set; }

    [CliOption("--vpn-site-link")]
    public string? VpnSiteLink { get; set; }

    [CliFlag("--with-link")]
    public bool? WithLink { get; set; }
}