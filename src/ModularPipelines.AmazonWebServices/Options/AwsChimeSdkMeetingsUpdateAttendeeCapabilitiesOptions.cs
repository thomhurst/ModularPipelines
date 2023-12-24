using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("chime-sdk-meetings", "update-attendee-capabilities")]
public record AwsChimeSdkMeetingsUpdateAttendeeCapabilitiesOptions(
[property: CommandSwitch("--meeting-id")] string MeetingId,
[property: CommandSwitch("--attendee-id")] string AttendeeId,
[property: CommandSwitch("--capabilities")] string Capabilities
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}