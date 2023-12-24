using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("elasticache", "describe-serverless-cache-snapshots")]
public record AwsElasticacheDescribeServerlessCacheSnapshotsOptions : AwsOptions
{
    [CommandSwitch("--serverless-cache-name")]
    public string? ServerlessCacheName { get; set; }

    [CommandSwitch("--serverless-cache-snapshot-name")]
    public string? ServerlessCacheSnapshotName { get; set; }

    [CommandSwitch("--snapshot-type")]
    public string? SnapshotType { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--page-size")]
    public int? PageSize { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}