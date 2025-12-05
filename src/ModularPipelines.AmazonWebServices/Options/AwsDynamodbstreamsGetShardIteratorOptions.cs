using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dynamodbstreams", "get-shard-iterator")]
public record AwsDynamodbstreamsGetShardIteratorOptions(
[property: CliOption("--stream-arn")] string StreamArn,
[property: CliOption("--shard-id")] string ShardId,
[property: CliOption("--shard-iterator-type")] string ShardIteratorType
) : AwsOptions
{
    [CliOption("--sequence-number")]
    public string? SequenceNumber { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}