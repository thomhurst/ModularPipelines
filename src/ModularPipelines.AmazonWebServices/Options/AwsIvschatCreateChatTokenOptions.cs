using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ivschat", "create-chat-token")]
public record AwsIvschatCreateChatTokenOptions(
[property: CommandSwitch("--room-identifier")] string RoomIdentifier,
[property: CommandSwitch("--user-id")] string UserId
) : AwsOptions
{
    [CommandSwitch("--attributes")]
    public IEnumerable<KeyValue>? Attributes { get; set; }

    [CommandSwitch("--capabilities")]
    public string[]? Capabilities { get; set; }

    [CommandSwitch("--session-duration-in-minutes")]
    public int? SessionDurationInMinutes { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}