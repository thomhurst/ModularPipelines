using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("rekognition", "detect-custom-labels")]
public record AwsRekognitionDetectCustomLabelsOptions(
[property: CommandSwitch("--project-version-arn")] string ProjectVersionArn
) : AwsOptions
{
    [CommandSwitch("--image")]
    public string? Image { get; set; }

    [CommandSwitch("--max-results")]
    public int? MaxResults { get; set; }

    [CommandSwitch("--min-confidence")]
    public float? MinConfidence { get; set; }

    [CommandSwitch("--image-bytes")]
    public string? ImageBytes { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}