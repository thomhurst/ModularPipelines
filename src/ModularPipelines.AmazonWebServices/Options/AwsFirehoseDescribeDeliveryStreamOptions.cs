using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("firehose", "describe-delivery-stream")]
public record AwsFirehoseDescribeDeliveryStreamOptions(
[property: CommandSwitch("--delivery-stream-name")] string DeliveryStreamName
) : AwsOptions
{
    [CommandSwitch("--limit")]
    public int? Limit { get; set; }

    [CommandSwitch("--exclusive-start-destination-id")]
    public string? ExclusiveStartDestinationId { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}