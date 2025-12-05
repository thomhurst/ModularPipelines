using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "delete-vpc-endpoint-service-configurations")]
public record AwsEc2DeleteVpcEndpointServiceConfigurationsOptions(
[property: CliOption("--service-ids")] string[] ServiceIds
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}