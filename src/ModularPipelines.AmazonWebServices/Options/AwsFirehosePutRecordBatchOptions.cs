using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("firehose", "put-record-batch")]
public record AwsFirehosePutRecordBatchOptions(
[property: CommandSwitch("--delivery-stream-name")] string DeliveryStreamName,
[property: CommandSwitch("--records")] string[] Records
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}