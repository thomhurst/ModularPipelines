using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("chime-sdk-meetings", "start-meeting-transcription")]
public record AwsChimeSdkMeetingsStartMeetingTranscriptionOptions(
[property: CommandSwitch("--meeting-id")] string MeetingId,
[property: CommandSwitch("--transcription-configuration")] string TranscriptionConfiguration
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}