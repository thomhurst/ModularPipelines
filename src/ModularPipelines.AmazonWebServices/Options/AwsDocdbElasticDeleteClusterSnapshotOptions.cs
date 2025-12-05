using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("docdb-elastic", "delete-cluster-snapshot")]
public record AwsDocdbElasticDeleteClusterSnapshotOptions(
[property: CliOption("--snapshot-arn")] string SnapshotArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}