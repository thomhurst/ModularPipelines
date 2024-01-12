using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("pubsub", "lite-topics", "update")]
public record GcloudPubsubLiteTopicsUpdateOptions(
[property: PositionalArgument] string Topic,
[property: PositionalArgument] string Location,
[property: CommandSwitch("--message-retention-period")] string MessageRetentionPeriod,
[property: CommandSwitch("--partitions")] string Partitions,
[property: CommandSwitch("--per-partition-bytes")] string PerPartitionBytes,
[property: CommandSwitch("--per-partition-publish-mib")] string PerPartitionPublishMib,
[property: CommandSwitch("--per-partition-subscribe-mib")] string PerPartitionSubscribeMib,
[property: CommandSwitch("--throughput-reservation")] string ThroughputReservation
) : GcloudOptions;