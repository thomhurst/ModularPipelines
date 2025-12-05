using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("rekognition", "start-segment-detection")]
public record AwsRekognitionStartSegmentDetectionOptions(
[property: CliOption("--video")] string Video,
[property: CliOption("--segment-types")] string[] SegmentTypes
) : AwsOptions
{
    [CliOption("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CliOption("--notification-channel")]
    public string? NotificationChannel { get; set; }

    [CliOption("--job-tag")]
    public string? JobTag { get; set; }

    [CliOption("--filters")]
    public string? Filters { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}