using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "reject-vpc-peering-connection")]
public record AwsEc2RejectVpcPeeringConnectionOptions(
[property: CommandSwitch("--vpc-peering-connection-id")] string VpcPeeringConnectionId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}