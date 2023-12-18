using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "vpn-site", "link", "add")]
public record AzNetworkVpnSiteLinkAddOptions(
[property: CommandSwitch("--ip-address")] string IpAddress,
[property: CommandSwitch("--name")] string Name
) : AzOptions
{
    [CommandSwitch("--asn")]
    public string? Asn { get; set; }

    [CommandSwitch("--bgp-peering-address")]
    public string? BgpPeeringAddress { get; set; }

    [CommandSwitch("--fqdn")]
    public string? Fqdn { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--link-provider-name")]
    public string? LinkProviderName { get; set; }

    [CommandSwitch("--link-speed-in-mbps")]
    public string? LinkSpeedInMbps { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--site-name")]
    public string? SiteName { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }
}