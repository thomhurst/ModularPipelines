using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("chime", "delete-room-membership")]
public record AwsChimeDeleteRoomMembershipOptions(
[property: CommandSwitch("--account-id")] string AccountId,
[property: CommandSwitch("--room-id")] string RoomId,
[property: CommandSwitch("--member-id")] string MemberId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}