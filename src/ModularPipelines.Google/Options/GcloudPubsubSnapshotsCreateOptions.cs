using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("pubsub", "snapshots", "create")]
public record GcloudPubsubSnapshotsCreateOptions(
[property: PositionalArgument] string Snapshot,
[property: CommandSwitch("--subscription")] string Subscription
) : GcloudOptions
{
    [CommandSwitch("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }

    [CommandSwitch("--subscription-project")]
    public string? SubscriptionProject { get; set; }
}