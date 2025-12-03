using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("elasticache", "modify-cache-cluster")]
public record AwsElasticacheModifyCacheClusterOptions(
[property: CliOption("--cache-cluster-id")] string CacheClusterId
) : AwsOptions
{
    [CliOption("--num-cache-nodes")]
    public int? NumCacheNodes { get; set; }

    [CliOption("--cache-node-ids-to-remove")]
    public string[]? CacheNodeIdsToRemove { get; set; }

    [CliOption("--az-mode")]
    public string? AzMode { get; set; }

    [CliOption("--new-availability-zones")]
    public string[]? NewAvailabilityZones { get; set; }

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

    [CliOption("--log-delivery-configurations")]
    public string[]? LogDeliveryConfigurations { get; set; }

    [CliOption("--ip-discovery")]
    public string? IpDiscovery { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}