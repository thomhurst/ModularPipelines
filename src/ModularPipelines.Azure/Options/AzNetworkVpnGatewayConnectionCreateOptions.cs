using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "vpn-gateway", "connection", "create")]
public record AzNetworkVpnGatewayConnectionCreateOptions(
[property: CommandSwitch("--gateway-name")] string GatewayName,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--remote-vpn-site")] string RemoteVpnSite,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--associated")]
    public string? Associated { get; set; }

    [CommandSwitch("--associated-inbound-routemap")]
    public string? AssociatedInboundRoutemap { get; set; }

    [CommandSwitch("--associated-outbound-routemap")]
    public string? AssociatedOutboundRoutemap { get; set; }

    [CommandSwitch("--connection-bandwidth")]
    public string? ConnectionBandwidth { get; set; }

    [BooleanCommandSwitch("--enable-bgp")]
    public bool? EnableBgp { get; set; }

    [BooleanCommandSwitch("--internet-security")]
    public bool? InternetSecurity { get; set; }

    [CommandSwitch("--labels")]
    public string? Labels { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--propagated")]
    public string? Propagated { get; set; }

    [CommandSwitch("--protocol-type")]
    public string? ProtocolType { get; set; }

    [BooleanCommandSwitch("--rate-limiting")]
    public bool? RateLimiting { get; set; }

    [CommandSwitch("--routing-weight")]
    public string? RoutingWeight { get; set; }

    [CommandSwitch("--shared-key")]
    public string? SharedKey { get; set; }

    [CommandSwitch("--vpn-site-link")]
    public string? VpnSiteLink { get; set; }

    [BooleanCommandSwitch("--with-link")]
    public bool? WithLink { get; set; }
}