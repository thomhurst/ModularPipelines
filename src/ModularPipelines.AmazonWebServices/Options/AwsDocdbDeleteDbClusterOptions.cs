using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("docdb", "delete-db-cluster")]
public record AwsDocdbDeleteDbClusterOptions(
[property: CliOption("--db-cluster-identifier")] string DbClusterIdentifier
) : AwsOptions
{
    [CliOption("--final-db-snapshot-identifier")]
    public string? FinalDbSnapshotIdentifier { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}