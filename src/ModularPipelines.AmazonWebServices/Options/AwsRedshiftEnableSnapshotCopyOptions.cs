using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("redshift", "enable-snapshot-copy")]
public record AwsRedshiftEnableSnapshotCopyOptions(
[property: CliOption("--cluster-identifier")] string ClusterIdentifier,
[property: CliOption("--destination-region")] string DestinationRegion
) : AwsOptions
{
    [CliOption("--retention-period")]
    public int? RetentionPeriod { get; set; }

    [CliOption("--snapshot-copy-grant-name")]
    public string? SnapshotCopyGrantName { get; set; }

    [CliOption("--manual-snapshot-retention-period")]
    public int? ManualSnapshotRetentionPeriod { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}