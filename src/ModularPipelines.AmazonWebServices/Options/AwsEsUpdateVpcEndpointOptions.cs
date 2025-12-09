using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("es", "update-vpc-endpoint")]
public record AwsEsUpdateVpcEndpointOptions(
[property: CliOption("--vpc-endpoint-id")] string VpcEndpointId,
[property: CliOption("--vpc-options")] string VpcOptions
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}