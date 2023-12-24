using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ivschat", "send-event")]
public record AwsIvschatSendEventOptions(
[property: CommandSwitch("--event-name")] string EventName,
[property: CommandSwitch("--room-identifier")] string RoomIdentifier
) : AwsOptions
{
    [CommandSwitch("--attributes")]
    public IEnumerable<KeyValue>? Attributes { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}