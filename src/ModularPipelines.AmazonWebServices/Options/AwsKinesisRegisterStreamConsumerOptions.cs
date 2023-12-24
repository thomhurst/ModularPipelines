using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("kinesis", "register-stream-consumer")]
public record AwsKinesisRegisterStreamConsumerOptions(
[property: CommandSwitch("--stream-arn")] string StreamArn,
[property: CommandSwitch("--consumer-name")] string ConsumerName
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}