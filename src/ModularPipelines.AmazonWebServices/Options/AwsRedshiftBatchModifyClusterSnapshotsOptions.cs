using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("redshift", "batch-modify-cluster-snapshots")]
public record AwsRedshiftBatchModifyClusterSnapshotsOptions(
[property: CommandSwitch("--snapshot-identifier-list")] string[] SnapshotIdentifierList
) : AwsOptions
{
    [CommandSwitch("--manual-snapshot-retention-period")]
    public int? ManualSnapshotRetentionPeriod { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}