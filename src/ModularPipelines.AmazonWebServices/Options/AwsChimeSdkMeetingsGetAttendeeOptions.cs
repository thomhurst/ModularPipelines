using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("chime-sdk-meetings", "get-attendee")]
public record AwsChimeSdkMeetingsGetAttendeeOptions(
[property: CommandSwitch("--meeting-id")] string MeetingId,
[property: CommandSwitch("--attendee-id")] string AttendeeId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}