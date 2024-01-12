using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "routers", "add-bgp-peer")]
public record GcloudComputeRoutersAddBgpPeerOptions(
[property: PositionalArgument] string Name,
[property: CommandSwitch("--interface")] string Interface,
[property: CommandSwitch("--peer-asn")] string PeerAsn,
[property: CommandSwitch("--peer-name")] string PeerName
) : GcloudOptions
{
    [CommandSwitch("--advertised-route-priority")]
    public string? AdvertisedRoutePriority { get; set; }

    [CommandSwitch("--advertisement-mode")]
    public string? AdvertisementMode { get; set; }

    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--custom-learned-route-priority")]
    public string? CustomLearnedRoutePriority { get; set; }

    [CommandSwitch("--[no-]enable-ipv6")]
    public string[]? NoEnableIpv6 { get; set; }

    [CommandSwitch("--[no-]enabled")]
    public string[]? NoEnabled { get; set; }

    [CommandSwitch("--instance")]
    public string? Instance { get; set; }

    [CommandSwitch("--instance-zone")]
    public string? InstanceZone { get; set; }

    [CommandSwitch("--ipv6-nexthop-address")]
    public string? Ipv6NexthopAddress { get; set; }

    [CommandSwitch("--md5-authentication-key")]
    public string? Md5AuthenticationKey { get; set; }

    [CommandSwitch("--peer-ip-address")]
    public string? PeerIpAddress { get; set; }

    [CommandSwitch("--peer-ipv6-nexthop-address")]
    public string? PeerIpv6NexthopAddress { get; set; }

    [CommandSwitch("--region")]
    public string? Region { get; set; }

    [CommandSwitch("--set-advertisement-groups")]
    public string[]? SetAdvertisementGroups { get; set; }

    [CommandSwitch("--set-advertisement-ranges")]
    public string[]? SetAdvertisementRanges { get; set; }

    [CommandSwitch("--set-custom-learned-route-ranges")]
    public string[]? SetCustomLearnedRouteRanges { get; set; }

    [CommandSwitch("--bfd-min-receive-interval")]
    public string? BfdMinReceiveInterval { get; set; }

    [CommandSwitch("--bfd-min-transmit-interval")]
    public string? BfdMinTransmitInterval { get; set; }

    [CommandSwitch("--bfd-multiplier")]
    public string? BfdMultiplier { get; set; }

    [CommandSwitch("--bfd-session-initialization-mode")]
    public string? BfdSessionInitializationMode { get; set; }

    [BooleanCommandSwitch("ACTIVE")]
    public bool? Active { get; set; }

    [BooleanCommandSwitch("DISABLED")]
    public bool? Disabled { get; set; }

    [BooleanCommandSwitch("PASSIVE")]
    public bool? Passive { get; set; }
}