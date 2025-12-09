using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "apply-security-groups-to-client-vpn-target-network")]
public record AwsEc2ApplySecurityGroupsToClientVpnTargetNetworkOptions(
[property: CliOption("--client-vpn-endpoint-id")] string ClientVpnEndpointId,
[property: CliOption("--vpc-id")] string VpcId,
[property: CliOption("--security-group-ids")] string[] SecurityGroupIds
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}