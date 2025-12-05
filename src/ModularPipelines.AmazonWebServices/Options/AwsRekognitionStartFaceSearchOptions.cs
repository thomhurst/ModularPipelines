using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("rekognition", "start-face-search")]
public record AwsRekognitionStartFaceSearchOptions(
[property: CliOption("--video")] string Video,
[property: CliOption("--collection-id")] string CollectionId
) : AwsOptions
{
    [CliOption("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CliOption("--face-match-threshold")]
    public float? FaceMatchThreshold { get; set; }

    [CliOption("--notification-channel")]
    public string? NotificationChannel { get; set; }

    [CliOption("--job-tag")]
    public string? JobTag { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}