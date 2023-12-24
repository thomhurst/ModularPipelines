using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("firehose", "stop-delivery-stream-encryption")]
public record AwsFirehoseStopDeliveryStreamEncryptionOptions(
[property: CommandSwitch("--delivery-stream-name")] string DeliveryStreamName
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}