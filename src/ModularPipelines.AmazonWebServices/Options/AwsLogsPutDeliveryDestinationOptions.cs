using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("logs", "put-delivery-destination")]
public record AwsLogsPutDeliveryDestinationOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--delivery-destination-configuration")] string DeliveryDestinationConfiguration
) : AwsOptions
{
    [CommandSwitch("--output-format")]
    public string? OutputFormat { get; set; }

    [CommandSwitch("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}