using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("chime-sdk-meetings", "batch-create-attendee")]
public record AwsChimeSdkMeetingsBatchCreateAttendeeOptions(
[property: CliOption("--meeting-id")] string MeetingId,
[property: CliOption("--attendees")] string[] Attendees
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}