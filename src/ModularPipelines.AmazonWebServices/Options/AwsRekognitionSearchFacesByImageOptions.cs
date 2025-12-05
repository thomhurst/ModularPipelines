using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("rekognition", "search-faces-by-image")]
public record AwsRekognitionSearchFacesByImageOptions(
[property: CliOption("--collection-id")] string CollectionId
) : AwsOptions
{
    [CliOption("--image")]
    public string? Image { get; set; }

    [CliOption("--max-faces")]
    public int? MaxFaces { get; set; }

    [CliOption("--face-match-threshold")]
    public float? FaceMatchThreshold { get; set; }

    [CliOption("--quality-filter")]
    public string? QualityFilter { get; set; }

    [CliOption("--image-bytes")]
    public string? ImageBytes { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}