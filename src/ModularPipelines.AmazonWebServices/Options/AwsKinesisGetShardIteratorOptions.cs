using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("kinesis", "get-shard-iterator")]
public record AwsKinesisGetShardIteratorOptions(
[property: CliOption("--shard-id")] string ShardId,
[property: CliOption("--shard-iterator-type")] string ShardIteratorType
) : AwsOptions
{
    [CliOption("--stream-name")]
    public string? StreamName { get; set; }

    [CliOption("--starting-sequence-number")]
    public string? StartingSequenceNumber { get; set; }

    [CliOption("--timestamp")]
    public long? Timestamp { get; set; }

    [CliOption("--stream-arn")]
    public string? StreamArn { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}