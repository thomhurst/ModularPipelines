using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("docdb-elastic", "create-cluster-snapshot")]
public record AwsDocdbElasticCreateClusterSnapshotOptions(
[property: CliOption("--cluster-arn")] string ClusterArn,
[property: CliOption("--snapshot-name")] string SnapshotName
) : AwsOptions
{
    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}