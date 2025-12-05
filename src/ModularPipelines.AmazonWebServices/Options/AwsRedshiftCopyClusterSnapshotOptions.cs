using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("redshift", "copy-cluster-snapshot")]
public record AwsRedshiftCopyClusterSnapshotOptions(
[property: CliOption("--source-snapshot-identifier")] string SourceSnapshotIdentifier,
[property: CliOption("--target-snapshot-identifier")] string TargetSnapshotIdentifier
) : AwsOptions
{
    [CliOption("--source-snapshot-cluster-identifier")]
    public string? SourceSnapshotClusterIdentifier { get; set; }

    [CliOption("--manual-snapshot-retention-period")]
    public int? ManualSnapshotRetentionPeriod { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}