using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("memorydb", "failover-shard")]
public record AwsMemorydbFailoverShardOptions(
[property: CliOption("--cluster-name")] string ClusterName,
[property: CliOption("--shard-name")] string ShardName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}