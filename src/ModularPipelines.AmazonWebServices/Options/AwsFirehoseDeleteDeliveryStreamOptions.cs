using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("firehose", "delete-delivery-stream")]
public record AwsFirehoseDeleteDeliveryStreamOptions(
[property: CliOption("--delivery-stream-name")] string DeliveryStreamName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}