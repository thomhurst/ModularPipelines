using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("rekognition", "start-face-search")]
public record AwsRekognitionStartFaceSearchOptions(
[property: CommandSwitch("--video")] string Video,
[property: CommandSwitch("--collection-id")] string CollectionId
) : AwsOptions
{
    [CommandSwitch("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CommandSwitch("--face-match-threshold")]
    public float? FaceMatchThreshold { get; set; }

    [CommandSwitch("--notification-channel")]
    public string? NotificationChannel { get; set; }

    [CommandSwitch("--job-tag")]
    public string? JobTag { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}