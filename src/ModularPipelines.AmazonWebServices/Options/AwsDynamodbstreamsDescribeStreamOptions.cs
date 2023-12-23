using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dynamodbstreams", "describe-stream")]
public record AwsDynamodbstreamsDescribeStreamOptions(
[property: CommandSwitch("--stream-arn")] string StreamArn
) : AwsOptions
{
    [CommandSwitch("--limit")]
    public int? Limit { get; set; }

    [CommandSwitch("--exclusive-start-shard-id")]
    public string? ExclusiveStartShardId { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}