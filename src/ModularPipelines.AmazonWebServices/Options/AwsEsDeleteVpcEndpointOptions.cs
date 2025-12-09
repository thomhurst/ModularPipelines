using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("es", "delete-vpc-endpoint")]
public record AwsEsDeleteVpcEndpointOptions(
[property: CliOption("--vpc-endpoint-id")] string VpcEndpointId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}