using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("kafka", "update-replication-info")]
public record AwsKafkaUpdateReplicationInfoOptions(
[property: CliOption("--current-version")] string CurrentVersion,
[property: CliOption("--replicator-arn")] string ReplicatorArn,
[property: CliOption("--source-kafka-cluster-arn")] string SourceKafkaClusterArn,
[property: CliOption("--target-kafka-cluster-arn")] string TargetKafkaClusterArn
) : AwsOptions
{
    [CliOption("--consumer-group-replication")]
    public string? ConsumerGroupReplication { get; set; }

    [CliOption("--topic-replication")]
    public string? TopicReplication { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}