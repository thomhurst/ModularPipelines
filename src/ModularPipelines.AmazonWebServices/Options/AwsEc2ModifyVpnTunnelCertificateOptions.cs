using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "modify-vpn-tunnel-certificate")]
public record AwsEc2ModifyVpnTunnelCertificateOptions(
[property: CommandSwitch("--vpn-connection-id")] string VpnConnectionId,
[property: CommandSwitch("--vpn-tunnel-outside-ip-address")] string VpnTunnelOutsideIpAddress
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}