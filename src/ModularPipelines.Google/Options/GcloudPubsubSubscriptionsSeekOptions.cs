using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("pubsub", "subscriptions", "seek")]
public record GcloudPubsubSubscriptionsSeekOptions(
[property: PositionalArgument] string Subscription,
[property: CommandSwitch("--snapshot")] string Snapshot,
[property: CommandSwitch("--time")] string Time
) : GcloudOptions
{
    [CommandSwitch("--snapshot-project")]
    public string? SnapshotProject { get; set; }
}