using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("redshift", "create-cluster-snapshot")]
public record AwsRedshiftCreateClusterSnapshotOptions(
[property: CommandSwitch("--snapshot-identifier")] string SnapshotIdentifier,
[property: CommandSwitch("--cluster-identifier")] string ClusterIdentifier
) : AwsOptions
{
    [CommandSwitch("--manual-snapshot-retention-period")]
    public int? ManualSnapshotRetentionPeriod { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}