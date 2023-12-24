using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("rekognition", "search-faces-by-image")]
public record AwsRekognitionSearchFacesByImageOptions(
[property: CommandSwitch("--collection-id")] string CollectionId
) : AwsOptions
{
    [CommandSwitch("--image")]
    public string? Image { get; set; }

    [CommandSwitch("--max-faces")]
    public int? MaxFaces { get; set; }

    [CommandSwitch("--face-match-threshold")]
    public float? FaceMatchThreshold { get; set; }

    [CommandSwitch("--quality-filter")]
    public string? QualityFilter { get; set; }

    [CommandSwitch("--image-bytes")]
    public string? ImageBytes { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}