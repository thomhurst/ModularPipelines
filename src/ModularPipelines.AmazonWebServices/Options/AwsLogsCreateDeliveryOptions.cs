using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("logs", "create-delivery")]
public record AwsLogsCreateDeliveryOptions(
[property: CliOption("--delivery-source-name")] string DeliverySourceName,
[property: CliOption("--delivery-destination-arn")] string DeliveryDestinationArn
) : AwsOptions
{
    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}