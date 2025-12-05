using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("elasticache", "create-serverless-cache")]
public record AwsElasticacheCreateServerlessCacheOptions(
[property: CliOption("--serverless-cache-name")] string ServerlessCacheName,
[property: CliOption("--engine")] string Engine
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--major-engine-version")]
    public string? MajorEngineVersion { get; set; }

    [CliOption("--cache-usage-limits")]
    public string? CacheUsageLimits { get; set; }

    [CliOption("--kms-key-id")]
    public string? KmsKeyId { get; set; }

    [CliOption("--security-group-ids")]
    public string[]? SecurityGroupIds { get; set; }

    [CliOption("--snapshot-arns-to-restore")]
    public string[]? SnapshotArnsToRestore { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--user-group-id")]
    public string? UserGroupId { get; set; }

    [CliOption("--subnet-ids")]
    public string[]? SubnetIds { get; set; }

    [CliOption("--snapshot-retention-limit")]
    public int? SnapshotRetentionLimit { get; set; }

    [CliOption("--daily-snapshot-time")]
    public string? DailySnapshotTime { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}