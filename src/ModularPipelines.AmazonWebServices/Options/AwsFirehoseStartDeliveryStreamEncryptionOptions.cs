using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("firehose", "start-delivery-stream-encryption")]
public record AwsFirehoseStartDeliveryStreamEncryptionOptions(
[property: CommandSwitch("--delivery-stream-name")] string DeliveryStreamName
) : AwsOptions
{
    [CommandSwitch("--delivery-stream-encryption-configuration-input")]
    public string? DeliveryStreamEncryptionConfigurationInput { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}