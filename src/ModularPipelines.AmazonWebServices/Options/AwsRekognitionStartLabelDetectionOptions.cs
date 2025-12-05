using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("rekognition", "start-label-detection")]
public record AwsRekognitionStartLabelDetectionOptions(
[property: CliOption("--video")] string Video
) : AwsOptions
{
    [CliOption("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CliOption("--min-confidence")]
    public float? MinConfidence { get; set; }

    [CliOption("--notification-channel")]
    public string? NotificationChannel { get; set; }

    [CliOption("--job-tag")]
    public string? JobTag { get; set; }

    [CliOption("--features")]
    public string[]? Features { get; set; }

    [CliOption("--settings")]
    public string? Settings { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}