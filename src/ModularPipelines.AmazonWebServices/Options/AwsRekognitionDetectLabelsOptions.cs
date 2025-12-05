using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("rekognition", "detect-labels")]
public record AwsRekognitionDetectLabelsOptions : AwsOptions
{
    [CliOption("--image")]
    public string? Image { get; set; }

    [CliOption("--max-labels")]
    public int? MaxLabels { get; set; }

    [CliOption("--min-confidence")]
    public float? MinConfidence { get; set; }

    [CliOption("--features")]
    public string[]? Features { get; set; }

    [CliOption("--settings")]
    public string? Settings { get; set; }

    [CliOption("--image-bytes")]
    public string? ImageBytes { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}