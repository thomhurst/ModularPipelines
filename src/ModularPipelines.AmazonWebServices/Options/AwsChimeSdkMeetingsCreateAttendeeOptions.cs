using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("chime-sdk-meetings", "create-attendee")]
public record AwsChimeSdkMeetingsCreateAttendeeOptions(
[property: CliOption("--meeting-id")] string MeetingId,
[property: CliOption("--external-user-id")] string ExternalUserId
) : AwsOptions
{
    [CliOption("--capabilities")]
    public string? Capabilities { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}