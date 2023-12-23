using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("memorydb", "failover-shard")]
public record AwsMemorydbFailoverShardOptions(
[property: CommandSwitch("--cluster-name")] string ClusterName,
[property: CommandSwitch("--shard-name")] string ShardName
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}