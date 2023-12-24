using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("chime-sdk-meetings", "batch-update-attendee-capabilities-except")]
public record AwsChimeSdkMeetingsBatchUpdateAttendeeCapabilitiesExceptOptions(
[property: CommandSwitch("--meeting-id")] string MeetingId,
[property: CommandSwitch("--excluded-attendee-ids")] string[] ExcludedAttendeeIds,
[property: CommandSwitch("--capabilities")] string Capabilities
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}