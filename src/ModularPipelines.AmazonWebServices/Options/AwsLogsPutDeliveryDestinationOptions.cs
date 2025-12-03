using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("logs", "put-delivery-destination")]
public record AwsLogsPutDeliveryDestinationOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--delivery-destination-configuration")] string DeliveryDestinationConfiguration
) : AwsOptions
{
    [CliOption("--output-format")]
    public string? OutputFormat { get; set; }

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}