using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("elasticache", "create-serverless-cache")]
public record AwsElasticacheCreateServerlessCacheOptions(
[property: CommandSwitch("--serverless-cache-name")] string ServerlessCacheName,
[property: CommandSwitch("--engine")] string Engine
) : AwsOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--major-engine-version")]
    public string? MajorEngineVersion { get; set; }

    [CommandSwitch("--cache-usage-limits")]
    public string? CacheUsageLimits { get; set; }

    [CommandSwitch("--kms-key-id")]
    public string? KmsKeyId { get; set; }

    [CommandSwitch("--security-group-ids")]
    public string[]? SecurityGroupIds { get; set; }

    [CommandSwitch("--snapshot-arns-to-restore")]
    public string[]? SnapshotArnsToRestore { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--user-group-id")]
    public string? UserGroupId { get; set; }

    [CommandSwitch("--subnet-ids")]
    public string[]? SubnetIds { get; set; }

    [CommandSwitch("--snapshot-retention-limit")]
    public int? SnapshotRetentionLimit { get; set; }

    [CommandSwitch("--daily-snapshot-time")]
    public string? DailySnapshotTime { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}