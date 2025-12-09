using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("opensearch", "update-vpc-endpoint")]
public record AwsOpensearchUpdateVpcEndpointOptions(
[property: CliOption("--vpc-endpoint-id")] string VpcEndpointId,
[property: CliOption("--vpc-options")] string VpcOptions
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}