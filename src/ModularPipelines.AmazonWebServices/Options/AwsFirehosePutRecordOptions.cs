using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("firehose", "put-record")]
public record AwsFirehosePutRecordOptions(
[property: CommandSwitch("--delivery-stream-name")] string DeliveryStreamName,
[property: CommandSwitch("--record")] string Record
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}