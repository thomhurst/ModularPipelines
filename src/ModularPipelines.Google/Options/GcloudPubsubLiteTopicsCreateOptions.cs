using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("pubsub", "lite-topics", "create")]
public record GcloudPubsubLiteTopicsCreateOptions(
[property: PositionalArgument] string Topic,
[property: CommandSwitch("--partitions")] string Partitions,
[property: CommandSwitch("--per-partition-bytes")] string PerPartitionBytes
) : GcloudOptions
{
    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [CommandSwitch("--message-retention-period")]
    public string? MessageRetentionPeriod { get; set; }

    [CommandSwitch("--per-partition-publish-mib")]
    public string? PerPartitionPublishMib { get; set; }

    [CommandSwitch("--per-partition-subscribe-mib")]
    public string? PerPartitionSubscribeMib { get; set; }

    [CommandSwitch("--throughput-reservation")]
    public string? ThroughputReservation { get; set; }
}