using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("logs", "create-delivery")]
public record AwsLogsCreateDeliveryOptions(
[property: CommandSwitch("--delivery-source-name")] string DeliverySourceName,
[property: CommandSwitch("--delivery-destination-arn")] string DeliveryDestinationArn
) : AwsOptions
{
    [CommandSwitch("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}