using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "replace-vpn-tunnel")]
public record AwsEc2ReplaceVpnTunnelOptions(
[property: CliOption("--vpn-connection-id")] string VpnConnectionId,
[property: CliOption("--vpn-tunnel-outside-ip-address")] string VpnTunnelOutsideIpAddress
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}