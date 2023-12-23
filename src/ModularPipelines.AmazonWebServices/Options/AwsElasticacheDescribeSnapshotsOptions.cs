using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("elasticache", "describe-snapshots")]
public record AwsElasticacheDescribeSnapshotsOptions : AwsOptions
{
    [CommandSwitch("--replication-group-id")]
    public string? ReplicationGroupId { get; set; }

    [CommandSwitch("--cache-cluster-id")]
    public string? CacheClusterId { get; set; }

    [CommandSwitch("--snapshot-name")]
    public string? SnapshotName { get; set; }

    [CommandSwitch("--snapshot-source")]
    public string? SnapshotSource { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--page-size")]
    public int? PageSize { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}