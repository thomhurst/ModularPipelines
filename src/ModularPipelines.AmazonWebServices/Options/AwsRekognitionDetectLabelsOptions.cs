using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("rekognition", "detect-labels")]
public record AwsRekognitionDetectLabelsOptions : AwsOptions
{
    [CommandSwitch("--image")]
    public string? Image { get; set; }

    [CommandSwitch("--max-labels")]
    public int? MaxLabels { get; set; }

    [CommandSwitch("--min-confidence")]
    public float? MinConfidence { get; set; }

    [CommandSwitch("--features")]
    public string[]? Features { get; set; }

    [CommandSwitch("--settings")]
    public string? Settings { get; set; }

    [CommandSwitch("--image-bytes")]
    public string? ImageBytes { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}