using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "instances", "network-interfaces", "update")]
public record GcloudComputeInstancesNetworkInterfacesUpdateOptions(
[property: PositionalArgument] string InstanceName
) : GcloudOptions
{
    [CommandSwitch("--aliases")]
    public string? Aliases { get; set; }

    [CommandSwitch("--external-ipv6-address")]
    public string? ExternalIpv6Address { get; set; }

    [CommandSwitch("--external-ipv6-prefix-length")]
    public string? ExternalIpv6PrefixLength { get; set; }

    [CommandSwitch("--internal-ipv6-address")]
    public string? InternalIpv6Address { get; set; }

    [CommandSwitch("--internal-ipv6-prefix-length")]
    public string? InternalIpv6PrefixLength { get; set; }

    [CommandSwitch("--ipv6-network-tier")]
    public string? Ipv6NetworkTier { get; set; }

    [CommandSwitch("--network")]
    public string? Network { get; set; }

    [CommandSwitch("--network-interface")]
    public string? NetworkInterface { get; set; }

    [CommandSwitch("--private-network-ip")]
    public string? PrivateNetworkIp { get; set; }

    [CommandSwitch("--security-policy")]
    public string? SecurityPolicy { get; set; }

    [CommandSwitch("--security-policy-region")]
    public string? SecurityPolicyRegion { get; set; }

    [CommandSwitch("--stack-type")]
    public string? StackType { get; set; }

    [CommandSwitch("--subnetwork")]
    public string? Subnetwork { get; set; }

    [CommandSwitch("--zone")]
    public string? Zone { get; set; }
}