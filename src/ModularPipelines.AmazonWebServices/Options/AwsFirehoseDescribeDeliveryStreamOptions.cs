using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("firehose", "describe-delivery-stream")]
public record AwsFirehoseDescribeDeliveryStreamOptions(
[property: CliOption("--delivery-stream-name")] string DeliveryStreamName
) : AwsOptions
{
    [CliOption("--limit")]
    public int? Limit { get; set; }

    [CliOption("--exclusive-start-destination-id")]
    public string? ExclusiveStartDestinationId { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}