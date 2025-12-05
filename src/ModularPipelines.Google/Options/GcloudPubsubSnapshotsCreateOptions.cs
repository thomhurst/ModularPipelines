using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("pubsub", "snapshots", "create")]
public record GcloudPubsubSnapshotsCreateOptions(
[property: CliArgument] string Snapshot,
[property: CliOption("--subscription")] string Subscription
) : GcloudOptions
{
    [CliOption("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }

    [CliOption("--subscription-project")]
    public string? SubscriptionProject { get; set; }
}