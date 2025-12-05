using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("redshift", "modify-snapshot-copy-retention-period")]
public record AwsRedshiftModifySnapshotCopyRetentionPeriodOptions(
[property: CliOption("--cluster-identifier")] string ClusterIdentifier,
[property: CliOption("--retention-period")] int RetentionPeriod
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}