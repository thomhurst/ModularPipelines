using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "apply-security-groups-to-client-vpn-target-network")]
public record AwsEc2ApplySecurityGroupsToClientVpnTargetNetworkOptions(
[property: CommandSwitch("--client-vpn-endpoint-id")] string ClientVpnEndpointId,
[property: CommandSwitch("--vpc-id")] string VpcId,
[property: CommandSwitch("--security-group-ids")] string[] SecurityGroupIds
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}