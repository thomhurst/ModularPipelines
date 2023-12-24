using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("rekognition", "start-face-detection")]
public record AwsRekognitionStartFaceDetectionOptions(
[property: CommandSwitch("--video")] string Video
) : AwsOptions
{
    [CommandSwitch("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CommandSwitch("--notification-channel")]
    public string? NotificationChannel { get; set; }

    [CommandSwitch("--face-attributes")]
    public string? FaceAttributes { get; set; }

    [CommandSwitch("--job-tag")]
    public string? JobTag { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}