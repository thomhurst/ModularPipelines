using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "modify-vpn-tunnel-options")]
public record AwsEc2ModifyVpnTunnelOptionsOptions(
[property: CliOption("--vpn-connection-id")] string VpnConnectionId,
[property: CliOption("--vpn-tunnel-outside-ip-address")] string VpnTunnelOutsideIpAddress,
[property: CliOption("--tunnel-options")] string TunnelOptions
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}