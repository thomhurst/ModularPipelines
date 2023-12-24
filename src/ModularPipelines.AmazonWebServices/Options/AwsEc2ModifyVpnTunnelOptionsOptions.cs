using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "modify-vpn-tunnel-options")]
public record AwsEc2ModifyVpnTunnelOptionsOptions(
[property: CommandSwitch("--vpn-connection-id")] string VpnConnectionId,
[property: CommandSwitch("--vpn-tunnel-outside-ip-address")] string VpnTunnelOutsideIpAddress,
[property: CommandSwitch("--tunnel-options")] string TunnelOptions
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}