using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("chime-sdk-meetings", "start-meeting-transcription")]
public record AwsChimeSdkMeetingsStartMeetingTranscriptionOptions(
[property: CliOption("--meeting-id")] string MeetingId,
[property: CliOption("--transcription-configuration")] string TranscriptionConfiguration
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}