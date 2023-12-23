using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("elasticache", "modify-replication-group")]
public record AwsElasticacheModifyReplicationGroupOptions(
[property: CommandSwitch("--replication-group-id")] string ReplicationGroupId
) : AwsOptions
{
    [CommandSwitch("--replication-group-description")]
    public string? ReplicationGroupDescription { get; set; }

    [CommandSwitch("--primary-cluster-id")]
    public string? PrimaryClusterId { get; set; }

    [CommandSwitch("--snapshotting-cluster-id")]
    public string? SnapshottingClusterId { get; set; }

    [CommandSwitch("--node-group-id")]
    public string? NodeGroupId { get; set; }

    [CommandSwitch("--cache-security-group-names")]
    public string[]? CacheSecurityGroupNames { get; set; }

    [CommandSwitch("--security-group-ids")]
    public string[]? SecurityGroupIds { get; set; }

    [CommandSwitch("--preferred-maintenance-window")]
    public string? PreferredMaintenanceWindow { get; set; }

    [CommandSwitch("--notification-topic-arn")]
    public string? NotificationTopicArn { get; set; }

    [CommandSwitch("--cache-parameter-group-name")]
    public string? CacheParameterGroupName { get; set; }

    [CommandSwitch("--notification-topic-status")]
    public string? NotificationTopicStatus { get; set; }

    [CommandSwitch("--engine-version")]
    public string? EngineVersion { get; set; }

    [CommandSwitch("--snapshot-retention-limit")]
    public int? SnapshotRetentionLimit { get; set; }

    [CommandSwitch("--snapshot-window")]
    public string? SnapshotWindow { get; set; }

    [CommandSwitch("--cache-node-type")]
    public string? CacheNodeType { get; set; }

    [CommandSwitch("--auth-token")]
    public string? AuthToken { get; set; }

    [CommandSwitch("--auth-token-update-strategy")]
    public string? AuthTokenUpdateStrategy { get; set; }

    [CommandSwitch("--user-group-ids-to-add")]
    public string[]? UserGroupIdsToAdd { get; set; }

    [CommandSwitch("--user-group-ids-to-remove")]
    public string[]? UserGroupIdsToRemove { get; set; }

    [CommandSwitch("--log-delivery-configurations")]
    public string[]? LogDeliveryConfigurations { get; set; }

    [CommandSwitch("--ip-discovery")]
    public string? IpDiscovery { get; set; }

    [CommandSwitch("--transit-encryption-mode")]
    public string? TransitEncryptionMode { get; set; }

    [CommandSwitch("--cluster-mode")]
    public string? ClusterMode { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}