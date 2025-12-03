using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "instances", "network-interfaces", "update")]
public record GcloudComputeInstancesNetworkInterfacesUpdateOptions(
[property: CliArgument] string InstanceName
) : GcloudOptions
{
    [CliOption("--aliases")]
    public string? Aliases { get; set; }

    [CliOption("--external-ipv6-address")]
    public string? ExternalIpv6Address { get; set; }

    [CliOption("--external-ipv6-prefix-length")]
    public string? ExternalIpv6PrefixLength { get; set; }

    [CliOption("--internal-ipv6-address")]
    public string? InternalIpv6Address { get; set; }

    [CliOption("--internal-ipv6-prefix-length")]
    public string? InternalIpv6PrefixLength { get; set; }

    [CliOption("--ipv6-network-tier")]
    public string? Ipv6NetworkTier { get; set; }

    [CliOption("--network")]
    public string? Network { get; set; }

    [CliOption("--network-interface")]
    public string? NetworkInterface { get; set; }

    [CliOption("--private-network-ip")]
    public string? PrivateNetworkIp { get; set; }

    [CliOption("--security-policy")]
    public string? SecurityPolicy { get; set; }

    [CliOption("--security-policy-region")]
    public string? SecurityPolicyRegion { get; set; }

    [CliOption("--stack-type")]
    public string? StackType { get; set; }

    [CliOption("--subnetwork")]
    public string? Subnetwork { get; set; }

    [CliOption("--zone")]
    public string? Zone { get; set; }
}