using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("redshift-serverless", "delete-snapshot-copy-configuration")]
public record AwsRedshiftServerlessDeleteSnapshotCopyConfigurationOptions(
[property: CliOption("--snapshot-copy-configuration-id")] string SnapshotCopyConfigurationId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}