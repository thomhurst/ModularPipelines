using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("chime", "batch-create-room-membership")]
public record AwsChimeBatchCreateRoomMembershipOptions(
[property: CommandSwitch("--account-id")] string AccountId,
[property: CommandSwitch("--room-id")] string RoomId,
[property: CommandSwitch("--membership-item-list")] string[] MembershipItemList
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}