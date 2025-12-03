using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("docdb-elastic", "get-cluster-snapshot")]
public record AwsDocdbElasticGetClusterSnapshotOptions(
[property: CliOption("--snapshot-arn")] string SnapshotArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}