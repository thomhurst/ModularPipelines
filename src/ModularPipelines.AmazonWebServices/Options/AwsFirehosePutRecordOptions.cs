using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("firehose", "put-record")]
public record AwsFirehosePutRecordOptions(
[property: CliOption("--delivery-stream-name")] string DeliveryStreamName,
[property: CliOption("--record")] string Record
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}