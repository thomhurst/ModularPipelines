using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "networks", "update")]
public record GcloudComputeNetworksUpdateOptions(
[property: PositionalArgument] string Name
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--[no-]enable-ula-internal-ipv6")]
    public string[]? NoEnableUlaInternalIpv6 { get; set; }

    [CommandSwitch("--internal-ipv6-range")]
    public string? InternalIpv6Range { get; set; }

    [CommandSwitch("--mtu")]
    public string? Mtu { get; set; }

    [CommandSwitch("--network-firewall-policy-enforcement-order")]
    public string? NetworkFirewallPolicyEnforcementOrder { get; set; }

    [CommandSwitch("--bgp-routing-mode")]
    public string? BgpRoutingMode { get; set; }

    [BooleanCommandSwitch("global")]
    public bool? Global { get; set; }

    [BooleanCommandSwitch("regional")]
    public bool? Regional { get; set; }

    [BooleanCommandSwitch("--switch-to-custom-subnet-mode")]
    public bool? SwitchToCustomSubnetMode { get; set; }
}