using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("firehose", "stop-delivery-stream-encryption")]
public record AwsFirehoseStopDeliveryStreamEncryptionOptions(
[property: CliOption("--delivery-stream-name")] string DeliveryStreamName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}