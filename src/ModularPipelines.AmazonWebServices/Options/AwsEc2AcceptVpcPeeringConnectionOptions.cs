using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "accept-vpc-peering-connection")]
public record AwsEc2AcceptVpcPeeringConnectionOptions(
[property: CommandSwitch("--vpc-peering-connection-id")] string VpcPeeringConnectionId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}