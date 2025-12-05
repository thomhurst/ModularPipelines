using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("redshift", "batch-modify-cluster-snapshots")]
public record AwsRedshiftBatchModifyClusterSnapshotsOptions(
[property: CliOption("--snapshot-identifier-list")] string[] SnapshotIdentifierList
) : AwsOptions
{
    [CliOption("--manual-snapshot-retention-period")]
    public int? ManualSnapshotRetentionPeriod { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}