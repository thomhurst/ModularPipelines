using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("pubsub", "lite-topics", "update")]
public record GcloudPubsubLiteTopicsUpdateOptions(
[property: CliArgument] string Topic,
[property: CliArgument] string Location,
[property: CliOption("--message-retention-period")] string MessageRetentionPeriod,
[property: CliOption("--partitions")] string Partitions,
[property: CliOption("--per-partition-bytes")] string PerPartitionBytes,
[property: CliOption("--per-partition-publish-mib")] string PerPartitionPublishMib,
[property: CliOption("--per-partition-subscribe-mib")] string PerPartitionSubscribeMib,
[property: CliOption("--throughput-reservation")] string ThroughputReservation
) : GcloudOptions;