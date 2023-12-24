using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("kinesis", "merge-shards")]
public record AwsKinesisMergeShardsOptions(
[property: CommandSwitch("--shard-to-merge")] string ShardToMerge,
[property: CommandSwitch("--adjacent-shard-to-merge")] string AdjacentShardToMerge
) : AwsOptions
{
    [CommandSwitch("--stream-name")]
    public string? StreamName { get; set; }

    [CommandSwitch("--stream-arn")]
    public string? StreamArn { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}