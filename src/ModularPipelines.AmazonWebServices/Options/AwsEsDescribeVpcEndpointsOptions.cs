using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("es", "describe-vpc-endpoints")]
public record AwsEsDescribeVpcEndpointsOptions(
[property: CliOption("--vpc-endpoint-ids")] string[] VpcEndpointIds
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}