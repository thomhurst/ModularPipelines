using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("elasticache", "create-cache-cluster")]
public record AwsElasticacheCreateCacheClusterOptions(
[property: CliOption("--cache-cluster-id")] string CacheClusterId
) : AwsOptions
{
    [CliOption("--replication-group-id")]
    public string? ReplicationGroupId { get; set; }

    [CliOption("--az-mode")]
    public string? AzMode { get; set; }

    [CliOption("--preferred-availability-zone")]
    public string? PreferredAvailabilityZone { get; set; }

    [CliOption("--preferred-availability-zones")]
    public string[]? PreferredAvailabilityZones { get; set; }

    [CliOption("--num-cache-nodes")]
    public int? NumCacheNodes { get; set; }

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

    [CliOption("--outpost-mode")]
    public string? OutpostMode { get; set; }

    [CliOption("--preferred-outpost-arn")]
    public string? PreferredOutpostArn { get; set; }

    [CliOption("--preferred-outpost-arns")]
    public string[]? PreferredOutpostArns { get; set; }

    [CliOption("--log-delivery-configurations")]
    public string[]? LogDeliveryConfigurations { get; set; }

    [CliOption("--network-type")]
    public string? NetworkType { get; set; }

    [CliOption("--ip-discovery")]
    public string? IpDiscovery { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}