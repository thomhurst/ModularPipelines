using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("memorydb", "delete-cluster")]
public record AwsMemorydbDeleteClusterOptions(
[property: CliOption("--cluster-name")] string ClusterName
) : AwsOptions
{
    [CliOption("--final-snapshot-name")]
    public string? FinalSnapshotName { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}