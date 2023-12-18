using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "vpn-site", "update")]
public record AzNetworkVpnSiteUpdateOptions : AzOptions
{
    [CommandSwitch("--add")]
    public string? Add { get; set; }

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

    [BooleanCommandSwitch("--force-string")]
    public bool? ForceString { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--ip-address")]
    public string? IpAddress { get; set; }

    [CommandSwitch("--link-speed")]
    public string? LinkSpeed { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--peer-weight")]
    public string? PeerWeight { get; set; }

    [CommandSwitch("--remove")]
    public string? Remove { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [BooleanCommandSwitch("--security-site")]
    public bool? SecuritySite { get; set; }

    [CommandSwitch("--set")]
    public string? Set { get; set; }

    [CommandSwitch("--site-key")]
    public string? SiteKey { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }

    [CommandSwitch("--virtual-wan")]
    public string? VirtualWan { get; set; }
}