using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "reject-vpc-endpoint-connections")]
public record AwsEc2RejectVpcEndpointConnectionsOptions(
[property: CliOption("--service-id")] string ServiceId,
[property: CliOption("--vpc-endpoint-ids")] string[] VpcEndpointIds
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}