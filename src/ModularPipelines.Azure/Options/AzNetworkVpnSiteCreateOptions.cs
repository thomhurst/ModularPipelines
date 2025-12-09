using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("network", "vpn-site", "create")]
public record AzNetworkVpnSiteCreateOptions(
[property: CliOption("--ip-address")] string IpAddress,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--address-prefixes")]
    public string? AddressPrefixes { get; set; }

    [CliOption("--asn")]
    public string? Asn { get; set; }

    [CliOption("--bgp-peering-address")]
    public string? BgpPeeringAddress { get; set; }

    [CliOption("--device-model")]
    public string? DeviceModel { get; set; }

    [CliOption("--device-vendor")]
    public string? DeviceVendor { get; set; }

    [CliOption("--link-speed")]
    public string? LinkSpeed { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--peer-weight")]
    public string? PeerWeight { get; set; }

    [CliFlag("--security-site")]
    public bool? SecuritySite { get; set; }

    [CliOption("--site-key")]
    public string? SiteKey { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliOption("--virtual-wan")]
    public string? VirtualWan { get; set; }

    [CliFlag("--with-link")]
    public bool? WithLink { get; set; }
}