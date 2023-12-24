using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("rekognition", "detect-text")]
public record AwsRekognitionDetectTextOptions : AwsOptions
{
    [CommandSwitch("--image")]
    public string? Image { get; set; }

    [CommandSwitch("--filters")]
    public string? Filters { get; set; }

    [CommandSwitch("--image-bytes")]
    public string? ImageBytes { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}