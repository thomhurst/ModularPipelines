using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("chime-sdk-meetings", "batch-create-attendee")]
public record AwsChimeSdkMeetingsBatchCreateAttendeeOptions(
[property: CommandSwitch("--meeting-id")] string MeetingId,
[property: CommandSwitch("--attendees")] string[] Attendees
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}