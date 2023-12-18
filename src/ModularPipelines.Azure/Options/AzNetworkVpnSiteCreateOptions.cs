using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "vpn-site", "create")]
public record AzNetworkVpnSiteCreateOptions(
[property: CommandSwitch("--ip-address")] string IpAddress,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--address-prefixes")]
    public string? AddressPrefixes { get; set; }

    [CommandSwitch("--asn")]
    public string? Asn { get; set; }

    [CommandSwitch("--bgp-peering-address")]
    public string? BgpPeeringAddress { get; set; }

    [CommandSwitch("--device-model")]
    public string? DeviceModel { get; set; }

    [CommandSwitch("--device-vendor")]
    public string? DeviceVendor { get; set; }

    [CommandSwitch("--link-speed")]
    public string? LinkSpeed { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--peer-weight")]
    public string? PeerWeight { get; set; }

    [BooleanCommandSwitch("--security-site")]
    public bool? SecuritySite { get; set; }

    [CommandSwitch("--site-key")]
    public string? SiteKey { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }

    [CommandSwitch("--virtual-wan")]
    public string? VirtualWan { get; set; }

    [BooleanCommandSwitch("--with-link")]
    public bool? WithLink { get; set; }
}