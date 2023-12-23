using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("elasticache", "modify-cache-cluster")]
public record AwsElasticacheModifyCacheClusterOptions(
[property: CommandSwitch("--cache-cluster-id")] string CacheClusterId
) : AwsOptions
{
    [CommandSwitch("--num-cache-nodes")]
    public int? NumCacheNodes { get; set; }

    [CommandSwitch("--cache-node-ids-to-remove")]
    public string[]? CacheNodeIdsToRemove { get; set; }

    [CommandSwitch("--az-mode")]
    public string? AzMode { get; set; }

    [CommandSwitch("--new-availability-zones")]
    public string[]? NewAvailabilityZones { get; set; }

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

    [CommandSwitch("--log-delivery-configurations")]
    public string[]? LogDeliveryConfigurations { get; set; }

    [CommandSwitch("--ip-discovery")]
    public string? IpDiscovery { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}