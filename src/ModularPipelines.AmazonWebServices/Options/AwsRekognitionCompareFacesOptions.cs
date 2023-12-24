using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("rekognition", "compare-faces")]
public record AwsRekognitionCompareFacesOptions : AwsOptions
{
    [CommandSwitch("--source-image")]
    public string? SourceImage { get; set; }

    [CommandSwitch("--target-image")]
    public string? TargetImage { get; set; }

    [CommandSwitch("--similarity-threshold")]
    public float? SimilarityThreshold { get; set; }

    [CommandSwitch("--quality-filter")]
    public string? QualityFilter { get; set; }

    [CommandSwitch("--source-image-bytes")]
    public string? SourceImageBytes { get; set; }

    [CommandSwitch("--target-image-bytes")]
    public string? TargetImageBytes { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}