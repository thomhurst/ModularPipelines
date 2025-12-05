using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("rekognition", "index-faces")]
public record AwsRekognitionIndexFacesOptions(
[property: CliOption("--collection-id")] string CollectionId
) : AwsOptions
{
    [CliOption("--image")]
    public string? Image { get; set; }

    [CliOption("--external-image-id")]
    public string? ExternalImageId { get; set; }

    [CliOption("--detection-attributes")]
    public string[]? DetectionAttributes { get; set; }

    [CliOption("--max-faces")]
    public int? MaxFaces { get; set; }

    [CliOption("--quality-filter")]
    public string? QualityFilter { get; set; }

    [CliOption("--image-bytes")]
    public string? ImageBytes { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}