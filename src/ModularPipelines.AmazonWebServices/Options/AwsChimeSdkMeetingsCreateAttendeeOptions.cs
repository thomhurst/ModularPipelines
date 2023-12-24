using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("chime-sdk-meetings", "create-attendee")]
public record AwsChimeSdkMeetingsCreateAttendeeOptions(
[property: CommandSwitch("--meeting-id")] string MeetingId,
[property: CommandSwitch("--external-user-id")] string ExternalUserId
) : AwsOptions
{
    [CommandSwitch("--capabilities")]
    public string? Capabilities { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}