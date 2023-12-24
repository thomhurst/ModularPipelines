using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("chime", "delete-room")]
public record AwsChimeDeleteRoomOptions(
[property: CommandSwitch("--account-id")] string AccountId,
[property: CommandSwitch("--room-id")] string RoomId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}