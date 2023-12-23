using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("kinesis", "get-shard-iterator")]
public record AwsKinesisGetShardIteratorOptions(
[property: CommandSwitch("--shard-id")] string ShardId,
[property: CommandSwitch("--shard-iterator-type")] string ShardIteratorType
) : AwsOptions
{
    [CommandSwitch("--stream-name")]
    public string? StreamName { get; set; }

    [CommandSwitch("--starting-sequence-number")]
    public string? StartingSequenceNumber { get; set; }

    [CommandSwitch("--timestamp")]
    public long? Timestamp { get; set; }

    [CommandSwitch("--stream-arn")]
    public string? StreamArn { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}