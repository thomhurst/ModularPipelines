using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("pubsub", "lite-subscriptions", "seek")]
public record GcloudPubsubLiteSubscriptionsSeekOptions(
[property: CliArgument] string Subscription,
[property: CliArgument] string Location,
[property: CliOption("--event-time")] string EventTime,
[property: CliOption("--publish-time")] string PublishTime,
[property: CliOption("--starting-offset")] string StartingOffset
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }
}