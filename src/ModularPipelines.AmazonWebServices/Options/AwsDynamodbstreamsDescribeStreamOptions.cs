using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dynamodbstreams", "describe-stream")]
public record AwsDynamodbstreamsDescribeStreamOptions(
[property: CliOption("--stream-arn")] string StreamArn
) : AwsOptions
{
    [CliOption("--limit")]
    public int? Limit { get; set; }

    [CliOption("--exclusive-start-shard-id")]
    public string? ExclusiveStartShardId { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}