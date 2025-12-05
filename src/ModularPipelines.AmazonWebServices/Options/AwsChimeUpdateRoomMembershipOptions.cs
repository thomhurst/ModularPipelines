using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("chime", "update-room-membership")]
public record AwsChimeUpdateRoomMembershipOptions(
[property: CliOption("--account-id")] string AccountId,
[property: CliOption("--room-id")] string RoomId,
[property: CliOption("--member-id")] string MemberId
) : AwsOptions
{
    [CliOption("--role")]
    public string? Role { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}