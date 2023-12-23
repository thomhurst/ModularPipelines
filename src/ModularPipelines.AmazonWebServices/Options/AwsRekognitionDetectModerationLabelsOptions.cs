using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("rekognition", "detect-moderation-labels")]
public record AwsRekognitionDetectModerationLabelsOptions : AwsOptions
{
    [CommandSwitch("--image")]
    public string? Image { get; set; }

    [CommandSwitch("--min-confidence")]
    public float? MinConfidence { get; set; }

    [CommandSwitch("--human-loop-config")]
    public string? HumanLoopConfig { get; set; }

    [CommandSwitch("--project-version")]
    public string? ProjectVersion { get; set; }

    [CommandSwitch("--image-bytes")]
    public string? ImageBytes { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}