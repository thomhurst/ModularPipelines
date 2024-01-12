using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "networks", "create")]
public record GcloudComputeNetworksCreateOptions(
[property: PositionalArgument] string Name
) : GcloudOptions
{
    [CommandSwitch("--bgp-routing-mode")]
    public string? BgpRoutingMode { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--[no-]enable-ula-internal-ipv6")]
    public string[]? NoEnableUlaInternalIpv6 { get; set; }

    [CommandSwitch("--internal-ipv6-range")]
    public string? InternalIpv6Range { get; set; }

    [CommandSwitch("--mtu")]
    public string? Mtu { get; set; }

    [CommandSwitch("--network-firewall-policy-enforcement-order")]
    public string? NetworkFirewallPolicyEnforcementOrder { get; set; }

    [CommandSwitch("--range")]
    public string? Range { get; set; }

    [CommandSwitch("--subnet-mode")]
    public string? SubnetMode { get; set; }
}