using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("redshift", "describe-cluster-snapshots")]
public record AwsRedshiftDescribeClusterSnapshotsOptions : AwsOptions
{
    [CliOption("--cluster-identifier")]
    public string? ClusterIdentifier { get; set; }

    [CliOption("--snapshot-identifier")]
    public string? SnapshotIdentifier { get; set; }

    [CliOption("--snapshot-arn")]
    public string? SnapshotArn { get; set; }

    [CliOption("--snapshot-type")]
    public string? SnapshotType { get; set; }

    [CliOption("--start-time")]
    public long? StartTime { get; set; }

    [CliOption("--end-time")]
    public long? EndTime { get; set; }

    [CliOption("--owner-account")]
    public string? OwnerAccount { get; set; }

    [CliOption("--tag-keys")]
    public string[]? TagKeys { get; set; }

    [CliOption("--tag-values")]
    public string[]? TagValues { get; set; }

    [CliOption("--sorting-entities")]
    public string[]? SortingEntities { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}