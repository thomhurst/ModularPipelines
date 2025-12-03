using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("memorydb", "create-cluster")]
public record AwsMemorydbCreateClusterOptions(
[property: CliOption("--cluster-name")] string ClusterName,
[property: CliOption("--node-type")] string NodeType,
[property: CliOption("--acl-name")] string AclName
) : AwsOptions
{
    [CliOption("--parameter-group-name")]
    public string? ParameterGroupName { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--num-shards")]
    public int? NumShards { get; set; }

    [CliOption("--num-replicas-per-shard")]
    public int? NumReplicasPerShard { get; set; }

    [CliOption("--subnet-group-name")]
    public string? SubnetGroupName { get; set; }

    [CliOption("--security-group-ids")]
    public string[]? SecurityGroupIds { get; set; }

    [CliOption("--maintenance-window")]
    public string? MaintenanceWindow { get; set; }

    [CliOption("--port")]
    public int? Port { get; set; }

    [CliOption("--sns-topic-arn")]
    public string? SnsTopicArn { get; set; }

    [CliOption("--kms-key-id")]
    public string? KmsKeyId { get; set; }

    [CliOption("--snapshot-arns")]
    public string[]? SnapshotArns { get; set; }

    [CliOption("--snapshot-name")]
    public string? SnapshotName { get; set; }

    [CliOption("--snapshot-retention-limit")]
    public int? SnapshotRetentionLimit { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--snapshot-window")]
    public string? SnapshotWindow { get; set; }

    [CliOption("--engine-version")]
    public string? EngineVersion { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}