using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("kafka", "update-replication-info")]
public record AwsKafkaUpdateReplicationInfoOptions(
[property: CommandSwitch("--current-version")] string CurrentVersion,
[property: CommandSwitch("--replicator-arn")] string ReplicatorArn,
[property: CommandSwitch("--source-kafka-cluster-arn")] string SourceKafkaClusterArn,
[property: CommandSwitch("--target-kafka-cluster-arn")] string TargetKafkaClusterArn
) : AwsOptions
{
    [CommandSwitch("--consumer-group-replication")]
    public string? ConsumerGroupReplication { get; set; }

    [CommandSwitch("--topic-replication")]
    public string? TopicReplication { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}