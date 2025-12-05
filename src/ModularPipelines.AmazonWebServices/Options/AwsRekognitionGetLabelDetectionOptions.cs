using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("rekognition", "get-label-detection")]
public record AwsRekognitionGetLabelDetectionOptions(
[property: CliOption("--job-id")] string JobId
) : AwsOptions
{
    [CliOption("--max-results")]
    public int? MaxResults { get; set; }

    [CliOption("--next-token")]
    public string? NextToken { get; set; }

    [CliOption("--sort-by")]
    public string? SortBy { get; set; }

    [CliOption("--aggregate-by")]
    public string? AggregateBy { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}