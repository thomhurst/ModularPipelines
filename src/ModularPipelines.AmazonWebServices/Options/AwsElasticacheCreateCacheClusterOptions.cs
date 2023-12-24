using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("elasticache", "create-cache-cluster")]
public record AwsElasticacheCreateCacheClusterOptions(
[property: CommandSwitch("--cache-cluster-id")] string CacheClusterId
) : AwsOptions
{
    [CommandSwitch("--replication-group-id")]
    public string? ReplicationGroupId { get; set; }

    [CommandSwitch("--az-mode")]
    public string? AzMode { get; set; }

    [CommandSwitch("--preferred-availability-zone")]
    public string? PreferredAvailabilityZone { get; set; }

    [CommandSwitch("--preferred-availability-zones")]
    public string[]? PreferredAvailabilityZones { get; set; }

    [CommandSwitch("--num-cache-nodes")]
    public int? NumCacheNodes { get; set; }

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

    [CommandSwitch("--outpost-mode")]
    public string? OutpostMode { get; set; }

    [CommandSwitch("--preferred-outpost-arn")]
    public string? PreferredOutpostArn { get; set; }

    [CommandSwitch("--preferred-outpost-arns")]
    public string[]? PreferredOutpostArns { get; set; }

    [CommandSwitch("--log-delivery-configurations")]
    public string[]? LogDeliveryConfigurations { get; set; }

    [CommandSwitch("--network-type")]
    public string? NetworkType { get; set; }

    [CommandSwitch("--ip-discovery")]
    public string? IpDiscovery { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}