using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("redshift", "delete-cluster")]
public record AwsRedshiftDeleteClusterOptions(
[property: CommandSwitch("--cluster-identifier")] string ClusterIdentifier
) : AwsOptions
{
    [CommandSwitch("--final-cluster-snapshot-identifier")]
    public string? FinalClusterSnapshotIdentifier { get; set; }

    [CommandSwitch("--final-cluster-snapshot-retention-period")]
    public int? FinalClusterSnapshotRetentionPeriod { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}