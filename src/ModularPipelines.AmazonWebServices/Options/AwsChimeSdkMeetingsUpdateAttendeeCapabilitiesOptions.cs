using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("chime-sdk-meetings", "update-attendee-capabilities")]
public record AwsChimeSdkMeetingsUpdateAttendeeCapabilitiesOptions(
[property: CliOption("--meeting-id")] string MeetingId,
[property: CliOption("--attendee-id")] string AttendeeId,
[property: CliOption("--capabilities")] string Capabilities
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}