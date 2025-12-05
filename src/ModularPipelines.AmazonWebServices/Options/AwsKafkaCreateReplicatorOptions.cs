using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("kafka", "create-replicator")]
public record AwsKafkaCreateReplicatorOptions(
[property: CliOption("--kafka-clusters")] string[] KafkaClusters,
[property: CliOption("--replication-info-list")] string[] ReplicationInfoList,
[property: CliOption("--replicator-name")] string ReplicatorName,
[property: CliOption("--service-execution-role-arn")] string ServiceExecutionRoleArn
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}