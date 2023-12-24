using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("kafka", "create-replicator")]
public record AwsKafkaCreateReplicatorOptions(
[property: CommandSwitch("--kafka-clusters")] string[] KafkaClusters,
[property: CommandSwitch("--replication-info-list")] string[] ReplicationInfoList,
[property: CommandSwitch("--replicator-name")] string ReplicatorName,
[property: CommandSwitch("--service-execution-role-arn")] string ServiceExecutionRoleArn
) : AwsOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}