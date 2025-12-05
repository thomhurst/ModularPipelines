using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("rekognition", "search-users-by-image")]
public record AwsRekognitionSearchUsersByImageOptions(
[property: CliOption("--collection-id")] string CollectionId
) : AwsOptions
{
    [CliOption("--image")]
    public string? Image { get; set; }

    [CliOption("--user-match-threshold")]
    public float? UserMatchThreshold { get; set; }

    [CliOption("--max-users")]
    public int? MaxUsers { get; set; }

    [CliOption("--quality-filter")]
    public string? QualityFilter { get; set; }

    [CliOption("--image-bytes")]
    public string? ImageBytes { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}