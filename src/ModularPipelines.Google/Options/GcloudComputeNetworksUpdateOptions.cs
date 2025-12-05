using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "networks", "update")]
public record GcloudComputeNetworksUpdateOptions(
[property: CliArgument] string Name
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--[no-]enable-ula-internal-ipv6")]
    public string[]? NoEnableUlaInternalIpv6 { get; set; }

    [CliOption("--internal-ipv6-range")]
    public string? InternalIpv6Range { get; set; }

    [CliOption("--mtu")]
    public string? Mtu { get; set; }

    [CliOption("--network-firewall-policy-enforcement-order")]
    public string? NetworkFirewallPolicyEnforcementOrder { get; set; }

    [CliOption("--bgp-routing-mode")]
    public string? BgpRoutingMode { get; set; }

    [CliFlag("global")]
    public bool? Global { get; set; }

    [CliFlag("regional")]
    public bool? Regional { get; set; }

    [CliFlag("--switch-to-custom-subnet-mode")]
    public bool? SwitchToCustomSubnetMode { get; set; }
}