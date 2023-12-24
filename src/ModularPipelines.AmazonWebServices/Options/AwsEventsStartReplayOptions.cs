using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("events", "start-replay")]
public record AwsEventsStartReplayOptions(
[property: CommandSwitch("--replay-name")] string ReplayName,
[property: CommandSwitch("--event-source-arn")] string EventSourceArn,
[property: CommandSwitch("--event-start-time")] long EventStartTime,
[property: CommandSwitch("--event-end-time")] long EventEndTime,
[property: CommandSwitch("--destination")] string Destination
) : AwsOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}