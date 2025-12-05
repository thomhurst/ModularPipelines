using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "delete-vpc-peering-connection")]
public record AwsEc2DeleteVpcPeeringConnectionOptions(
[property: CliOption("--vpc-peering-connection-id")] string VpcPeeringConnectionId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}