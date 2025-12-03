using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("b2bi", "get-transformer-job")]
public record AwsB2biGetTransformerJobOptions(
[property: CliOption("--transformer-job-id")] string TransformerJobId,
[property: CliOption("--transformer-id")] string TransformerId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}