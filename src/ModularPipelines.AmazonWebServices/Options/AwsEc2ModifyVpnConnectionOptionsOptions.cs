using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "modify-vpn-connection-options")]
public record AwsEc2ModifyVpnConnectionOptionsOptions(
[property: CliOption("--vpn-connection-id")] string VpnConnectionId
) : AwsOptions
{
    [CliOption("--local-ipv4-network-cidr")]
    public string? LocalIpv4NetworkCidr { get; set; }

    [CliOption("--remote-ipv4-network-cidr")]
    public string? RemoteIpv4NetworkCidr { get; set; }

    [CliOption("--local-ipv6-network-cidr")]
    public string? LocalIpv6NetworkCidr { get; set; }

    [CliOption("--remote-ipv6-network-cidr")]
    public string? RemoteIpv6NetworkCidr { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}