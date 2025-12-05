using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("rds", "describe-db-cluster-snapshot-attributes")]
public record AwsRdsDescribeDbClusterSnapshotAttributesOptions(
[property: CliOption("--db-cluster-snapshot-identifier")] string DbClusterSnapshotIdentifier
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}