using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("opensearch", "delete-vpc-endpoint")]
public record AwsOpensearchDeleteVpcEndpointOptions(
[property: CliOption("--vpc-endpoint-id")] string VpcEndpointId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}