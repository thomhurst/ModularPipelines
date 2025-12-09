using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("redshift", "batch-delete-cluster-snapshots")]
public record AwsRedshiftBatchDeleteClusterSnapshotsOptions(
[property: CliOption("--identifiers")] string[] Identifiers
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}