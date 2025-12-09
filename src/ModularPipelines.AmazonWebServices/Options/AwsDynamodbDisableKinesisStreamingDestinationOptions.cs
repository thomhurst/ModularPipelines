using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dynamodb", "disable-kinesis-streaming-destination")]
public record AwsDynamodbDisableKinesisStreamingDestinationOptions(
[property: CliOption("--table-name")] string TableName,
[property: CliOption("--stream-arn")] string StreamArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}