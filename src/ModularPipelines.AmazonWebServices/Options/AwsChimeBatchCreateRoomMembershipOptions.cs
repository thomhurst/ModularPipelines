using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("chime", "batch-create-room-membership")]
public record AwsChimeBatchCreateRoomMembershipOptions(
[property: CliOption("--account-id")] string AccountId,
[property: CliOption("--room-id")] string RoomId,
[property: CliOption("--membership-item-list")] string[] MembershipItemList
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}