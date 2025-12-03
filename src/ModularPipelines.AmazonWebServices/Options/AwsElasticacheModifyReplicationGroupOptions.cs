using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("elasticache", "modify-replication-group")]
public record AwsElasticacheModifyReplicationGroupOptions(
[property: CliOption("--replication-group-id")] string ReplicationGroupId
) : AwsOptions
{
    [CliOption("--replication-group-description")]
    public string? ReplicationGroupDescription { get; set; }

    [CliOption("--primary-cluster-id")]
    public string? PrimaryClusterId { get; set; }

    [CliOption("--snapshotting-cluster-id")]
    public string? SnapshottingClusterId { get; set; }

    [CliOption("--node-group-id")]
    public string? NodeGroupId { get; set; }

    [CliOption("--cache-security-group-names")]
    public string[]? CacheSecurityGroupNames { get; set; }

    [CliOption("--security-group-ids")]
    public string[]? SecurityGroupIds { get; set; }

    [CliOption("--preferred-maintenance-window")]
    public string? PreferredMaintenanceWindow { get; set; }

    [CliOption("--notification-topic-arn")]
    public string? NotificationTopicArn { get; set; }

    [CliOption("--cache-parameter-group-name")]
    public string? CacheParameterGroupName { get; set; }

    [CliOption("--notification-topic-status")]
    public string? NotificationTopicStatus { get; set; }

    [CliOption("--engine-version")]
    public string? EngineVersion { get; set; }

    [CliOption("--snapshot-retention-limit")]
    public int? SnapshotRetentionLimit { get; set; }

    [CliOption("--snapshot-window")]
    public string? SnapshotWindow { get; set; }

    [CliOption("--cache-node-type")]
    public string? CacheNodeType { get; set; }

    [CliOption("--auth-token")]
    public string? AuthToken { get; set; }

    [CliOption("--auth-token-update-strategy")]
    public string? AuthTokenUpdateStrategy { get; set; }

    [CliOption("--user-group-ids-to-add")]
    public string[]? UserGroupIdsToAdd { get; set; }

    [CliOption("--user-group-ids-to-remove")]
    public string[]? UserGroupIdsToRemove { get; set; }

    [CliOption("--log-delivery-configurations")]
    public string[]? LogDeliveryConfigurations { get; set; }

    [CliOption("--ip-discovery")]
    public string? IpDiscovery { get; set; }

    [CliOption("--transit-encryption-mode")]
    public string? TransitEncryptionMode { get; set; }

    [CliOption("--cluster-mode")]
    public string? ClusterMode { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}