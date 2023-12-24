using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("chime", "redact-room-message")]
public record AwsChimeRedactRoomMessageOptions(
[property: CommandSwitch("--account-id")] string AccountId,
[property: CommandSwitch("--room-id")] string RoomId,
[property: CommandSwitch("--message-id")] string MessageId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}