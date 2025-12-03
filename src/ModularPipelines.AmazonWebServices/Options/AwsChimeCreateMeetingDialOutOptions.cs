using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("chime", "create-meeting-dial-out")]
public record AwsChimeCreateMeetingDialOutOptions(
[property: CliOption("--meeting-id")] string MeetingId,
[property: CliOption("--from-phone-number")] string FromPhoneNumber,
[property: CliOption("--to-phone-number")] string ToPhoneNumber,
[property: CliOption("--join-token")] string JoinToken
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}