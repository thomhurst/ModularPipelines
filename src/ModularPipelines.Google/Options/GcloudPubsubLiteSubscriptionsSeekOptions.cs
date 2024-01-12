using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("pubsub", "lite-subscriptions", "seek")]
public record GcloudPubsubLiteSubscriptionsSeekOptions(
[property: PositionalArgument] string Subscription,
[property: PositionalArgument] string Location,
[property: CommandSwitch("--event-time")] string EventTime,
[property: CommandSwitch("--publish-time")] string PublishTime,
[property: CommandSwitch("--starting-offset")] string StartingOffset
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }
}