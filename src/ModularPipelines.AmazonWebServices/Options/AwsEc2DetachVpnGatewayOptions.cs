using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "detach-vpn-gateway")]
public record AwsEc2DetachVpnGatewayOptions(
[property: CommandSwitch("--vpc-id")] string VpcId,
[property: CommandSwitch("--vpn-gateway-id")] string VpnGatewayId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}