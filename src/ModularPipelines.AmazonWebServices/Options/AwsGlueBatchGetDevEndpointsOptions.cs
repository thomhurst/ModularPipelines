using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("glue", "batch-get-dev-endpoints")]
public record AwsGlueBatchGetDevEndpointsOptions(
[property: CliOption("--dev-endpoint-names")] string[] DevEndpointNames
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}