using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("redshift", "copy-cluster-snapshot")]
public record AwsRedshiftCopyClusterSnapshotOptions(
[property: CommandSwitch("--source-snapshot-identifier")] string SourceSnapshotIdentifier,
[property: CommandSwitch("--target-snapshot-identifier")] string TargetSnapshotIdentifier
) : AwsOptions
{
    [CommandSwitch("--source-snapshot-cluster-identifier")]
    public string? SourceSnapshotClusterIdentifier { get; set; }

    [CommandSwitch("--manual-snapshot-retention-period")]
    public int? ManualSnapshotRetentionPeriod { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}