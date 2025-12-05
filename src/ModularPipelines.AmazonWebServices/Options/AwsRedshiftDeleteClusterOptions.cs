using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("redshift", "delete-cluster")]
public record AwsRedshiftDeleteClusterOptions(
[property: CliOption("--cluster-identifier")] string ClusterIdentifier
) : AwsOptions
{
    [CliOption("--final-cluster-snapshot-identifier")]
    public string? FinalClusterSnapshotIdentifier { get; set; }

    [CliOption("--final-cluster-snapshot-retention-period")]
    public int? FinalClusterSnapshotRetentionPeriod { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}