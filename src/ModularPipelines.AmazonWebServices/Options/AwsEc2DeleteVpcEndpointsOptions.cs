using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "delete-vpc-endpoints")]
public record AwsEc2DeleteVpcEndpointsOptions(
[property: CliOption("--vpc-endpoint-ids")] string[] VpcEndpointIds
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}