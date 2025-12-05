using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("kinesis", "split-shard")]
public record AwsKinesisSplitShardOptions(
[property: CliOption("--shard-to-split")] string ShardToSplit,
[property: CliOption("--new-starting-hash-key")] string NewStartingHashKey
) : AwsOptions
{
    [CliOption("--stream-name")]
    public string? StreamName { get; set; }

    [CliOption("--stream-arn")]
    public string? StreamArn { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}