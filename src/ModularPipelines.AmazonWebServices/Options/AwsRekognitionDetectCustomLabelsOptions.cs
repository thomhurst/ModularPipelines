using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("rekognition", "detect-custom-labels")]
public record AwsRekognitionDetectCustomLabelsOptions(
[property: CliOption("--project-version-arn")] string ProjectVersionArn
) : AwsOptions
{
    [CliOption("--image")]
    public string? Image { get; set; }

    [CliOption("--max-results")]
    public int? MaxResults { get; set; }

    [CliOption("--min-confidence")]
    public float? MinConfidence { get; set; }

    [CliOption("--image-bytes")]
    public string? ImageBytes { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}