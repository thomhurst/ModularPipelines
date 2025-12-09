using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("redshift-serverless", "delete-namespace")]
public record AwsRedshiftServerlessDeleteNamespaceOptions(
[property: CliOption("--namespace-name")] string NamespaceName
) : AwsOptions
{
    [CliOption("--final-snapshot-name")]
    public string? FinalSnapshotName { get; set; }

    [CliOption("--final-snapshot-retention-period")]
    public int? FinalSnapshotRetentionPeriod { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}