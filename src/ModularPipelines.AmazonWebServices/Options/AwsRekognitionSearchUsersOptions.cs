using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("rekognition", "search-users")]
public record AwsRekognitionSearchUsersOptions(
[property: CliOption("--collection-id")] string CollectionId
) : AwsOptions
{
    [CliOption("--user-id")]
    public string? UserId { get; set; }

    [CliOption("--face-id")]
    public string? FaceId { get; set; }

    [CliOption("--user-match-threshold")]
    public float? UserMatchThreshold { get; set; }

    [CliOption("--max-users")]
    public int? MaxUsers { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}