using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("chime-sdk-meetings", "stop-meeting-transcription")]
public record AwsChimeSdkMeetingsStopMeetingTranscriptionOptions(
[property: CliOption("--meeting-id")] string MeetingId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}