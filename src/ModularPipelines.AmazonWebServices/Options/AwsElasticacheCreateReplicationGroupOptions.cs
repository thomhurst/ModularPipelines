using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("elasticache", "create-replication-group")]
public record AwsElasticacheCreateReplicationGroupOptions(
[property: CliOption("--replication-group-id")] string ReplicationGroupId,
[property: CliOption("--replication-group-description")] string ReplicationGroupDescription
) : AwsOptions
{
    [CliOption("--global-replication-group-id")]
    public string? GlobalReplicationGroupId { get; set; }

    [CliOption("--primary-cluster-id")]
    public string? PrimaryClusterId { get; set; }

    [CliOption("--num-cache-clusters")]
    public int? NumCacheClusters { get; set; }

    [CliOption("--preferred-cache-cluster-azs")]
    public string[]? PreferredCacheClusterAzs { get; set; }

    [CliOption("--num-node-groups")]
    public int? NumNodeGroups { get; set; }

    [CliOption("--replicas-per-node-group")]
    public int? ReplicasPerNodeGroup { get; set; }

    [CliOption("--node-group-configuration")]
    public string[]? NodeGroupConfiguration { get; set; }

    [CliOption("--cache-node-type")]
    public string? CacheNodeType { get; set; }

    [CliOption("--engine")]
    public string? Engine { get; set; }

    [CliOption("--engine-version")]
    public string? EngineVersion { get; set; }

    [CliOption("--cache-parameter-group-name")]
    public string? CacheParameterGroupName { get; set; }

    [CliOption("--cache-subnet-group-name")]
    public string? CacheSubnetGroupName { get; set; }

    [CliOption("--cache-security-group-names")]
    public string[]? CacheSecurityGroupNames { get; set; }

    [CliOption("--security-group-ids")]
    public string[]? SecurityGroupIds { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--snapshot-arns")]
    public string[]? SnapshotArns { get; set; }

    [CliOption("--snapshot-name")]
    public string? SnapshotName { get; set; }

    [CliOption("--preferred-maintenance-window")]
    public string? PreferredMaintenanceWindow { get; set; }

    [CliOption("--port")]
    public int? Port { get; set; }

    [CliOption("--notification-topic-arn")]
    public string? NotificationTopicArn { get; set; }

    [CliOption("--snapshot-retention-limit")]
    public int? SnapshotRetentionLimit { get; set; }

    [CliOption("--snapshot-window")]
    public string? SnapshotWindow { get; set; }

    [CliOption("--auth-token")]
    public string? AuthToken { get; set; }

    [CliOption("--kms-key-id")]
    public string? KmsKeyId { get; set; }

    [CliOption("--user-group-ids")]
    public string[]? UserGroupIds { get; set; }

    [CliOption("--log-delivery-configurations")]
    public string[]? LogDeliveryConfigurations { get; set; }

    [CliOption("--network-type")]
    public string? NetworkType { get; set; }

    [CliOption("--ip-discovery")]
    public string? IpDiscovery { get; set; }

    [CliOption("--transit-encryption-mode")]
    public string? TransitEncryptionMode { get; set; }

    [CliOption("--cluster-mode")]
    public string? ClusterMode { get; set; }

    [CliOption("--serverless-cache-snapshot-name")]
    public string? ServerlessCacheSnapshotName { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}