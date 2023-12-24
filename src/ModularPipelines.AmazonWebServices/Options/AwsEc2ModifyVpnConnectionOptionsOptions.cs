using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "modify-vpn-connection-options")]
public record AwsEc2ModifyVpnConnectionOptionsOptions(
[property: CommandSwitch("--vpn-connection-id")] string VpnConnectionId
) : AwsOptions
{
    [CommandSwitch("--local-ipv4-network-cidr")]
    public string? LocalIpv4NetworkCidr { get; set; }

    [CommandSwitch("--remote-ipv4-network-cidr")]
    public string? RemoteIpv4NetworkCidr { get; set; }

    [CommandSwitch("--local-ipv6-network-cidr")]
    public string? LocalIpv6NetworkCidr { get; set; }

    [CommandSwitch("--remote-ipv6-network-cidr")]
    public string? RemoteIpv6NetworkCidr { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}