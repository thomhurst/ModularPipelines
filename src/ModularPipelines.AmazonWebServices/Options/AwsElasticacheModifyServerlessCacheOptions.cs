using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("elasticache", "modify-serverless-cache")]
public record AwsElasticacheModifyServerlessCacheOptions(
[property: CliOption("--serverless-cache-name")] string ServerlessCacheName
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--cache-usage-limits")]
    public string? CacheUsageLimits { get; set; }

    [CliOption("--user-group-id")]
    public string? UserGroupId { get; set; }

    [CliOption("--security-group-ids")]
    public string[]? SecurityGroupIds { get; set; }

    [CliOption("--snapshot-retention-limit")]
    public int? SnapshotRetentionLimit { get; set; }

    [CliOption("--daily-snapshot-time")]
    public string? DailySnapshotTime { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}