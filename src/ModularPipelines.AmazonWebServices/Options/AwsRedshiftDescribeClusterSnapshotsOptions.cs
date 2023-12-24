using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("redshift", "describe-cluster-snapshots")]
public record AwsRedshiftDescribeClusterSnapshotsOptions : AwsOptions
{
    [CommandSwitch("--cluster-identifier")]
    public string? ClusterIdentifier { get; set; }

    [CommandSwitch("--snapshot-identifier")]
    public string? SnapshotIdentifier { get; set; }

    [CommandSwitch("--snapshot-arn")]
    public string? SnapshotArn { get; set; }

    [CommandSwitch("--snapshot-type")]
    public string? SnapshotType { get; set; }

    [CommandSwitch("--start-time")]
    public long? StartTime { get; set; }

    [CommandSwitch("--end-time")]
    public long? EndTime { get; set; }

    [CommandSwitch("--owner-account")]
    public string? OwnerAccount { get; set; }

    [CommandSwitch("--tag-keys")]
    public string[]? TagKeys { get; set; }

    [CommandSwitch("--tag-values")]
    public string[]? TagValues { get; set; }

    [CommandSwitch("--sorting-entities")]
    public string[]? SortingEntities { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--page-size")]
    public int? PageSize { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}