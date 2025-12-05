using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("redshift-serverless", "create-snapshot-copy-configuration")]
public record AwsRedshiftServerlessCreateSnapshotCopyConfigurationOptions(
[property: CliOption("--destination-region")] string DestinationRegion,
[property: CliOption("--namespace-name")] string NamespaceName
) : AwsOptions
{
    [CliOption("--destination-kms-key-id")]
    public string? DestinationKmsKeyId { get; set; }

    [CliOption("--snapshot-retention-period")]
    public int? SnapshotRetentionPeriod { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}