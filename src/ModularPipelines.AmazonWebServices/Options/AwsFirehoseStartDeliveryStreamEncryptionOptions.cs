using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("firehose", "start-delivery-stream-encryption")]
public record AwsFirehoseStartDeliveryStreamEncryptionOptions(
[property: CliOption("--delivery-stream-name")] string DeliveryStreamName
) : AwsOptions
{
    [CliOption("--delivery-stream-encryption-configuration-input")]
    public string? DeliveryStreamEncryptionConfigurationInput { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}