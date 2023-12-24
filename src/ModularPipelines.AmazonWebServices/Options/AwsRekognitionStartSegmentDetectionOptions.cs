using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("rekognition", "start-segment-detection")]
public record AwsRekognitionStartSegmentDetectionOptions(
[property: CommandSwitch("--video")] string Video,
[property: CommandSwitch("--segment-types")] string[] SegmentTypes
) : AwsOptions
{
    [CommandSwitch("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CommandSwitch("--notification-channel")]
    public string? NotificationChannel { get; set; }

    [CommandSwitch("--job-tag")]
    public string? JobTag { get; set; }

    [CommandSwitch("--filters")]
    public string? Filters { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}