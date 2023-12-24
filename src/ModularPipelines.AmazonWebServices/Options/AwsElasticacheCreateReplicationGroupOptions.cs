using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("elasticache", "create-replication-group")]
public record AwsElasticacheCreateReplicationGroupOptions(
[property: CommandSwitch("--replication-group-id")] string ReplicationGroupId,
[property: CommandSwitch("--replication-group-description")] string ReplicationGroupDescription
) : AwsOptions
{
    [CommandSwitch("--global-replication-group-id")]
    public string? GlobalReplicationGroupId { get; set; }

    [CommandSwitch("--primary-cluster-id")]
    public string? PrimaryClusterId { get; set; }

    [CommandSwitch("--num-cache-clusters")]
    public int? NumCacheClusters { get; set; }

    [CommandSwitch("--preferred-cache-cluster-azs")]
    public string[]? PreferredCacheClusterAzs { get; set; }

    [CommandSwitch("--num-node-groups")]
    public int? NumNodeGroups { get; set; }

    [CommandSwitch("--replicas-per-node-group")]
    public int? ReplicasPerNodeGroup { get; set; }

    [CommandSwitch("--node-group-configuration")]
    public string[]? NodeGroupConfiguration { get; set; }

    [CommandSwitch("--cache-node-type")]
    public string? CacheNodeType { get; set; }

    [CommandSwitch("--engine")]
    public string? Engine { get; set; }

    [CommandSwitch("--engine-version")]
    public string? EngineVersion { get; set; }

    [CommandSwitch("--cache-parameter-group-name")]
    public string? CacheParameterGroupName { get; set; }

    [CommandSwitch("--cache-subnet-group-name")]
    public string? CacheSubnetGroupName { get; set; }

    [CommandSwitch("--cache-security-group-names")]
    public string[]? CacheSecurityGroupNames { get; set; }

    [CommandSwitch("--security-group-ids")]
    public string[]? SecurityGroupIds { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--snapshot-arns")]
    public string[]? SnapshotArns { get; set; }

    [CommandSwitch("--snapshot-name")]
    public string? SnapshotName { get; set; }

    [CommandSwitch("--preferred-maintenance-window")]
    public string? PreferredMaintenanceWindow { get; set; }

    [CommandSwitch("--port")]
    public int? Port { get; set; }

    [CommandSwitch("--notification-topic-arn")]
    public string? NotificationTopicArn { get; set; }

    [CommandSwitch("--snapshot-retention-limit")]
    public int? SnapshotRetentionLimit { get; set; }

    [CommandSwitch("--snapshot-window")]
    public string? SnapshotWindow { get; set; }

    [CommandSwitch("--auth-token")]
    public string? AuthToken { get; set; }

    [CommandSwitch("--kms-key-id")]
    public string? KmsKeyId { get; set; }

    [CommandSwitch("--user-group-ids")]
    public string[]? UserGroupIds { get; set; }

    [CommandSwitch("--log-delivery-configurations")]
    public string[]? LogDeliveryConfigurations { get; set; }

    [CommandSwitch("--network-type")]
    public string? NetworkType { get; set; }

    [CommandSwitch("--ip-discovery")]
    public string? IpDiscovery { get; set; }

    [CommandSwitch("--transit-encryption-mode")]
    public string? TransitEncryptionMode { get; set; }

    [CommandSwitch("--cluster-mode")]
    public string? ClusterMode { get; set; }

    [CommandSwitch("--serverless-cache-snapshot-name")]
    public string? ServerlessCacheSnapshotName { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}