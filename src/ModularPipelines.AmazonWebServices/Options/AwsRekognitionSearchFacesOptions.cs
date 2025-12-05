using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("rekognition", "search-faces")]
public record AwsRekognitionSearchFacesOptions(
[property: CliOption("--collection-id")] string CollectionId,
[property: CliOption("--face-id")] string FaceId
) : AwsOptions
{
    [CliOption("--max-faces")]
    public int? MaxFaces { get; set; }

    [CliOption("--face-match-threshold")]
    public float? FaceMatchThreshold { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}