using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("rekognition", "detect-moderation-labels")]
public record AwsRekognitionDetectModerationLabelsOptions : AwsOptions
{
    [CliOption("--image")]
    public string? Image { get; set; }

    [CliOption("--min-confidence")]
    public float? MinConfidence { get; set; }

    [CliOption("--human-loop-config")]
    public string? HumanLoopConfig { get; set; }

    [CliOption("--project-version")]
    public string? ProjectVersion { get; set; }

    [CliOption("--image-bytes")]
    public string? ImageBytes { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}