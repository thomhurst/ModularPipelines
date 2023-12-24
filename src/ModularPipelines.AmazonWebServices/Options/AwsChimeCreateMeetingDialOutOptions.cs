using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("chime", "create-meeting-dial-out")]
public record AwsChimeCreateMeetingDialOutOptions(
[property: CommandSwitch("--meeting-id")] string MeetingId,
[property: CommandSwitch("--from-phone-number")] string FromPhoneNumber,
[property: CommandSwitch("--to-phone-number")] string ToPhoneNumber,
[property: CommandSwitch("--join-token")] string JoinToken
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}