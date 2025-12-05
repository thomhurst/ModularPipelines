using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("neptune", "describe-db-cluster-snapshot-attributes")]
public record AwsNeptuneDescribeDbClusterSnapshotAttributesOptions(
[property: CliOption("--db-cluster-snapshot-identifier")] string DbClusterSnapshotIdentifier
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}