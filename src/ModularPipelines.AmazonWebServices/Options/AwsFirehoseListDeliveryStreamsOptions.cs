using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("firehose", "list-delivery-streams")]
public record AwsFirehoseListDeliveryStreamsOptions : AwsOptions
{
    [CliOption("--limit")]
    public int? Limit { get; set; }

    [CliOption("--delivery-stream-type")]
    public string? DeliveryStreamType { get; set; }

    [CliOption("--exclusive-start-delivery-stream-name")]
    public string? ExclusiveStartDeliveryStreamName { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}