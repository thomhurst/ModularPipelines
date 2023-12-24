using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dynamodbstreams", "get-shard-iterator")]
public record AwsDynamodbstreamsGetShardIteratorOptions(
[property: CommandSwitch("--stream-arn")] string StreamArn,
[property: CommandSwitch("--shard-id")] string ShardId,
[property: CommandSwitch("--shard-iterator-type")] string ShardIteratorType
) : AwsOptions
{
    [CommandSwitch("--sequence-number")]
    public string? SequenceNumber { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}