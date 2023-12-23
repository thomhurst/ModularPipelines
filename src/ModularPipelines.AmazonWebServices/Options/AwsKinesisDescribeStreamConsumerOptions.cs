using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("kinesis", "describe-stream-consumer")]
public record AwsKinesisDescribeStreamConsumerOptions : AwsOptions
{
    [CommandSwitch("--stream-arn")]
    public string? StreamArn { get; set; }

    [CommandSwitch("--consumer-name")]
    public string? ConsumerName { get; set; }

    [CommandSwitch("--consumer-arn")]
    public string? ConsumerArn { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}