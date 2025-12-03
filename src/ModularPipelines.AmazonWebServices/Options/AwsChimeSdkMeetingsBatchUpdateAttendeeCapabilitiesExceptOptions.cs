using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("chime-sdk-meetings", "batch-update-attendee-capabilities-except")]
public record AwsChimeSdkMeetingsBatchUpdateAttendeeCapabilitiesExceptOptions(
[property: CliOption("--meeting-id")] string MeetingId,
[property: CliOption("--excluded-attendee-ids")] string[] ExcludedAttendeeIds,
[property: CliOption("--capabilities")] string Capabilities
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}