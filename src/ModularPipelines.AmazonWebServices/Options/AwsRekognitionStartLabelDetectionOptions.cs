using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("rekognition", "start-label-detection")]
public record AwsRekognitionStartLabelDetectionOptions(
[property: CommandSwitch("--video")] string Video
) : AwsOptions
{
    [CommandSwitch("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CommandSwitch("--min-confidence")]
    public float? MinConfidence { get; set; }

    [CommandSwitch("--notification-channel")]
    public string? NotificationChannel { get; set; }

    [CommandSwitch("--job-tag")]
    public string? JobTag { get; set; }

    [CommandSwitch("--features")]
    public string[]? Features { get; set; }

    [CommandSwitch("--settings")]
    public string? Settings { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}