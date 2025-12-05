using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("redshift-serverless", "update-snapshot-copy-configuration")]
public record AwsRedshiftServerlessUpdateSnapshotCopyConfigurationOptions(
[property: CliOption("--snapshot-copy-configuration-id")] string SnapshotCopyConfigurationId
) : AwsOptions
{
    [CliOption("--snapshot-retention-period")]
    public int? SnapshotRetentionPeriod { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}