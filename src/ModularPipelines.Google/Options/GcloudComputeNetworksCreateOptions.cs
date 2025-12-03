using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "networks", "create")]
public record GcloudComputeNetworksCreateOptions(
[property: CliArgument] string Name
) : GcloudOptions
{
    [CliOption("--bgp-routing-mode")]
    public string? BgpRoutingMode { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--[no-]enable-ula-internal-ipv6")]
    public string[]? NoEnableUlaInternalIpv6 { get; set; }

    [CliOption("--internal-ipv6-range")]
    public string? InternalIpv6Range { get; set; }

    [CliOption("--mtu")]
    public string? Mtu { get; set; }

    [CliOption("--network-firewall-policy-enforcement-order")]
    public string? NetworkFirewallPolicyEnforcementOrder { get; set; }

    [CliOption("--range")]
    public string? Range { get; set; }

    [CliOption("--subnet-mode")]
    public string? SubnetMode { get; set; }
}