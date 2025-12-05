using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("kinesis", "describe-stream-consumer")]
public record AwsKinesisDescribeStreamConsumerOptions : AwsOptions
{
    [CliOption("--stream-arn")]
    public string? StreamArn { get; set; }

    [CliOption("--consumer-name")]
    public string? ConsumerName { get; set; }

    [CliOption("--consumer-arn")]
    public string? ConsumerArn { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}