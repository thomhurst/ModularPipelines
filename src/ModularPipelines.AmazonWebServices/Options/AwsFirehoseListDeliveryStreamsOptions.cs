using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("firehose", "list-delivery-streams")]
public record AwsFirehoseListDeliveryStreamsOptions : AwsOptions
{
    [CommandSwitch("--limit")]
    public int? Limit { get; set; }

    [CommandSwitch("--delivery-stream-type")]
    public string? DeliveryStreamType { get; set; }

    [CommandSwitch("--exclusive-start-delivery-stream-name")]
    public string? ExclusiveStartDeliveryStreamName { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}