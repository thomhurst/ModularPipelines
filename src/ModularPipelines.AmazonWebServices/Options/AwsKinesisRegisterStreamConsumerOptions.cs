using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("kinesis", "register-stream-consumer")]
public record AwsKinesisRegisterStreamConsumerOptions(
[property: CliOption("--stream-arn")] string StreamArn,
[property: CliOption("--consumer-name")] string ConsumerName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}