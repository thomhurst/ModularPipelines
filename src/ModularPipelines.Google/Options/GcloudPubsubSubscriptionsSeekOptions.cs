using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("pubsub", "subscriptions", "seek")]
public record GcloudPubsubSubscriptionsSeekOptions(
[property: CliArgument] string Subscription,
[property: CliOption("--snapshot")] string Snapshot,
[property: CliOption("--time")] string Time
) : GcloudOptions
{
    [CliOption("--snapshot-project")]
    public string? SnapshotProject { get; set; }
}