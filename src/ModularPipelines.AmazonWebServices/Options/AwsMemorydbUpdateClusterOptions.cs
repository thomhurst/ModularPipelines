using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("memorydb", "update-cluster")]
public record AwsMemorydbUpdateClusterOptions(
[property: CommandSwitch("--cluster-name")] string ClusterName
) : AwsOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--security-group-ids")]
    public string[]? SecurityGroupIds { get; set; }

    [CommandSwitch("--maintenance-window")]
    public string? MaintenanceWindow { get; set; }

    [CommandSwitch("--sns-topic-arn")]
    public string? SnsTopicArn { get; set; }

    [CommandSwitch("--sns-topic-status")]
    public string? SnsTopicStatus { get; set; }

    [CommandSwitch("--parameter-group-name")]
    public string? ParameterGroupName { get; set; }

    [CommandSwitch("--snapshot-window")]
    public string? SnapshotWindow { get; set; }

    [CommandSwitch("--snapshot-retention-limit")]
    public int? SnapshotRetentionLimit { get; set; }

    [CommandSwitch("--node-type")]
    public string? NodeType { get; set; }

    [CommandSwitch("--engine-version")]
    public string? EngineVersion { get; set; }

    [CommandSwitch("--replica-configuration")]
    public string? ReplicaConfiguration { get; set; }

    [CommandSwitch("--shard-configuration")]
    public string? ShardConfiguration { get; set; }

    [CommandSwitch("--acl-name")]
    public string? AclName { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}