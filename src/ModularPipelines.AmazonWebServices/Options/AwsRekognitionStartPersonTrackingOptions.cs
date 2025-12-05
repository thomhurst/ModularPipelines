using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("rekognition", "start-person-tracking")]
public record AwsRekognitionStartPersonTrackingOptions(
[property: CliOption("--video")] string Video
) : AwsOptions
{
    [CliOption("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CliOption("--notification-channel")]
    public string? NotificationChannel { get; set; }

    [CliOption("--job-tag")]
    public string? JobTag { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}