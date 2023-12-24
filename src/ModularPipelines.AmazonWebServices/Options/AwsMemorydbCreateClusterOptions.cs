using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("memorydb", "create-cluster")]
public record AwsMemorydbCreateClusterOptions(
[property: CommandSwitch("--cluster-name")] string ClusterName,
[property: CommandSwitch("--node-type")] string NodeType,
[property: CommandSwitch("--acl-name")] string AclName
) : AwsOptions
{
    [CommandSwitch("--parameter-group-name")]
    public string? ParameterGroupName { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--num-shards")]
    public int? NumShards { get; set; }

    [CommandSwitch("--num-replicas-per-shard")]
    public int? NumReplicasPerShard { get; set; }

    [CommandSwitch("--subnet-group-name")]
    public string? SubnetGroupName { get; set; }

    [CommandSwitch("--security-group-ids")]
    public string[]? SecurityGroupIds { get; set; }

    [CommandSwitch("--maintenance-window")]
    public string? MaintenanceWindow { get; set; }

    [CommandSwitch("--port")]
    public int? Port { get; set; }

    [CommandSwitch("--sns-topic-arn")]
    public string? SnsTopicArn { get; set; }

    [CommandSwitch("--kms-key-id")]
    public string? KmsKeyId { get; set; }

    [CommandSwitch("--snapshot-arns")]
    public string[]? SnapshotArns { get; set; }

    [CommandSwitch("--snapshot-name")]
    public string? SnapshotName { get; set; }

    [CommandSwitch("--snapshot-retention-limit")]
    public int? SnapshotRetentionLimit { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--snapshot-window")]
    public string? SnapshotWindow { get; set; }

    [CommandSwitch("--engine-version")]
    public string? EngineVersion { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}