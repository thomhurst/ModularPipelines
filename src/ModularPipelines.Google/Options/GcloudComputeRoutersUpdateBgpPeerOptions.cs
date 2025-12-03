using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "routers", "update-bgp-peer")]
public record GcloudComputeRoutersUpdateBgpPeerOptions(
[property: CliArgument] string Name,
[property: CliOption("--peer-name")] string PeerName
) : GcloudOptions
{
    [CliOption("--advertised-route-priority")]
    public string? AdvertisedRoutePriority { get; set; }

    [CliOption("--advertisement-mode")]
    public string? AdvertisementMode { get; set; }

    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliFlag("--clear-md5-authentication-key")]
    public bool? ClearMd5AuthenticationKey { get; set; }

    [CliOption("--custom-learned-route-priority")]
    public string? CustomLearnedRoutePriority { get; set; }

    [CliOption("--[no-]enable-ipv6")]
    public string[]? NoEnableIpv6 { get; set; }

    [CliOption("--[no-]enabled")]
    public string[]? NoEnabled { get; set; }

    [CliOption("--interface")]
    public string? Interface { get; set; }

    [CliOption("--ip-address")]
    public string? IpAddress { get; set; }

    [CliOption("--ipv6-nexthop-address")]
    public string? Ipv6NexthopAddress { get; set; }

    [CliOption("--md5-authentication-key")]
    public string? Md5AuthenticationKey { get; set; }

    [CliOption("--peer-asn")]
    public string? PeerAsn { get; set; }

    [CliOption("--peer-ip-address")]
    public string? PeerIpAddress { get; set; }

    [CliOption("--peer-ipv6-nexthop-address")]
    public string? PeerIpv6NexthopAddress { get; set; }

    [CliOption("--region")]
    public string? Region { get; set; }

    [CliOption("--set-advertisement-groups")]
    public string[]? SetAdvertisementGroups { get; set; }

    [CliOption("--set-advertisement-ranges")]
    public string[]? SetAdvertisementRanges { get; set; }

    [CliOption("--set-custom-learned-route-ranges")]
    public string[]? SetCustomLearnedRouteRanges { get; set; }

    [CliOption("--add-advertisement-groups")]
    public string[]? AddAdvertisementGroups { get; set; }

    [CliFlag("ALL_SUBNETS")]
    public bool? AllSubnets { get; set; }

    [CliOption("--add-advertisement-ranges")]
    public string[]? AddAdvertisementRanges { get; set; }

    [CliOption("--remove-advertisement-groups")]
    public string[]? RemoveAdvertisementGroups { get; set; }

    [CliOption("--remove-advertisement-ranges")]
    public string[]? RemoveAdvertisementRanges { get; set; }

    [CliOption("--add-custom-learned-route-ranges")]
    public string[]? AddCustomLearnedRouteRanges { get; set; }

    [CliOption("--remove-custom-learned-route-ranges")]
    public string[]? RemoveCustomLearnedRouteRanges { get; set; }

    [CliOption("--bfd-min-receive-interval")]
    public string? BfdMinReceiveInterval { get; set; }

    [CliOption("--bfd-min-transmit-interval")]
    public string? BfdMinTransmitInterval { get; set; }

    [CliOption("--bfd-multiplier")]
    public string? BfdMultiplier { get; set; }

    [CliOption("--bfd-session-initialization-mode")]
    public string? BfdSessionInitializationMode { get; set; }

    [CliFlag("ACTIVE")]
    public bool? Active { get; set; }

    [CliFlag("DISABLED")]
    public bool? Disabled { get; set; }

    [CliFlag("PASSIVE")]
    public bool? Passive { get; set; }
}