using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("memorydb", "batch-update-cluster")]
public record AwsMemorydbBatchUpdateClusterOptions(
[property: CliOption("--cluster-names")] string[] ClusterNames
) : AwsOptions
{
    [CliOption("--service-update")]
    public string? ServiceUpdate { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}