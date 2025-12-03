using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("pubsub", "lite-topics", "create")]
public record GcloudPubsubLiteTopicsCreateOptions(
[property: CliArgument] string Topic,
[property: CliOption("--partitions")] string Partitions,
[property: CliOption("--per-partition-bytes")] string PerPartitionBytes
) : GcloudOptions
{
    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--message-retention-period")]
    public string? MessageRetentionPeriod { get; set; }

    [CliOption("--per-partition-publish-mib")]
    public string? PerPartitionPublishMib { get; set; }

    [CliOption("--per-partition-subscribe-mib")]
    public string? PerPartitionSubscribeMib { get; set; }

    [CliOption("--throughput-reservation")]
    public string? ThroughputReservation { get; set; }
}