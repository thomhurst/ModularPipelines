using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("chime-sdk-meetings", "get-attendee")]
public record AwsChimeSdkMeetingsGetAttendeeOptions(
[property: CliOption("--meeting-id")] string MeetingId,
[property: CliOption("--attendee-id")] string AttendeeId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}