using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("memorydb", "update-cluster")]
public record AwsMemorydbUpdateClusterOptions(
[property: CliOption("--cluster-name")] string ClusterName
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--security-group-ids")]
    public string[]? SecurityGroupIds { get; set; }

    [CliOption("--maintenance-window")]
    public string? MaintenanceWindow { get; set; }

    [CliOption("--sns-topic-arn")]
    public string? SnsTopicArn { get; set; }

    [CliOption("--sns-topic-status")]
    public string? SnsTopicStatus { get; set; }

    [CliOption("--parameter-group-name")]
    public string? ParameterGroupName { get; set; }

    [CliOption("--snapshot-window")]
    public string? SnapshotWindow { get; set; }

    [CliOption("--snapshot-retention-limit")]
    public int? SnapshotRetentionLimit { get; set; }

    [CliOption("--node-type")]
    public string? NodeType { get; set; }

    [CliOption("--engine-version")]
    public string? EngineVersion { get; set; }

    [CliOption("--replica-configuration")]
    public string? ReplicaConfiguration { get; set; }

    [CliOption("--shard-configuration")]
    public string? ShardConfiguration { get; set; }

    [CliOption("--acl-name")]
    public string? AclName { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}