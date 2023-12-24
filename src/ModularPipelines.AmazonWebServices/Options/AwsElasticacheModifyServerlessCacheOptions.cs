using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("elasticache", "modify-serverless-cache")]
public record AwsElasticacheModifyServerlessCacheOptions(
[property: CommandSwitch("--serverless-cache-name")] string ServerlessCacheName
) : AwsOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--cache-usage-limits")]
    public string? CacheUsageLimits { get; set; }

    [CommandSwitch("--user-group-id")]
    public string? UserGroupId { get; set; }

    [CommandSwitch("--security-group-ids")]
    public string[]? SecurityGroupIds { get; set; }

    [CommandSwitch("--snapshot-retention-limit")]
    public int? SnapshotRetentionLimit { get; set; }

    [CommandSwitch("--daily-snapshot-time")]
    public string? DailySnapshotTime { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}