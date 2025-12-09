using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("rds", "describe-db-snapshot-attributes")]
public record AwsRdsDescribeDbSnapshotAttributesOptions(
[property: CliOption("--db-snapshot-identifier")] string DbSnapshotIdentifier
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}