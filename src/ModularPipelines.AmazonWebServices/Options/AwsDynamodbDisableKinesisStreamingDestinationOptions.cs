using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dynamodb", "disable-kinesis-streaming-destination")]
public record AwsDynamodbDisableKinesisStreamingDestinationOptions(
[property: CommandSwitch("--table-name")] string TableName,
[property: CommandSwitch("--stream-arn")] string StreamArn
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}