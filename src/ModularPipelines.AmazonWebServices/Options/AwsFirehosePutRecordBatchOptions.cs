using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("firehose", "put-record-batch")]
public record AwsFirehosePutRecordBatchOptions(
[property: CliOption("--delivery-stream-name")] string DeliveryStreamName,
[property: CliOption("--records")] string[] Records
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}