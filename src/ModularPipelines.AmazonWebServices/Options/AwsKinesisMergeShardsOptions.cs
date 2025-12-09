using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("kinesis", "merge-shards")]
public record AwsKinesisMergeShardsOptions(
[property: CliOption("--shard-to-merge")] string ShardToMerge,
[property: CliOption("--adjacent-shard-to-merge")] string AdjacentShardToMerge
) : AwsOptions
{
    [CliOption("--stream-name")]
    public string? StreamName { get; set; }

    [CliOption("--stream-arn")]
    public string? StreamArn { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}