using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("network", "vpn-site", "link", "add")]
public record AzNetworkVpnSiteLinkAddOptions(
[property: CliOption("--ip-address")] string IpAddress,
[property: CliOption("--name")] string Name
) : AzOptions
{
    [CliOption("--asn")]
    public string? Asn { get; set; }

    [CliOption("--bgp-peering-address")]
    public string? BgpPeeringAddress { get; set; }

    [CliOption("--fqdn")]
    public string? Fqdn { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--link-provider-name")]
    public string? LinkProviderName { get; set; }

    [CliOption("--link-speed-in-mbps")]
    public string? LinkSpeedInMbps { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--site-name")]
    public string? SiteName { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}