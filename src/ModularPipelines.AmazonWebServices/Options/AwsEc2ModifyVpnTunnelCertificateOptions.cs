using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "modify-vpn-tunnel-certificate")]
public record AwsEc2ModifyVpnTunnelCertificateOptions(
[property: CliOption("--vpn-connection-id")] string VpnConnectionId,
[property: CliOption("--vpn-tunnel-outside-ip-address")] string VpnTunnelOutsideIpAddress
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}