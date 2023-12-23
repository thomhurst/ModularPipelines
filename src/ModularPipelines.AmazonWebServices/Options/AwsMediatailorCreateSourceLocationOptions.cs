using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("mediatailor", "create-source-location")]
public record AwsMediatailorCreateSourceLocationOptions(
[property: CommandSwitch("--http-configuration")] string HttpConfiguration,
[property: CommandSwitch("--source-location-name")] string SourceLocationName
) : AwsOptions
{
    [CommandSwitch("--access-configuration")]
    public string? AccessConfiguration { get; set; }

    [CommandSwitch("--default-segment-delivery-configuration")]
    public string? DefaultSegmentDeliveryConfiguration { get; set; }

    [CommandSwitch("--segment-delivery-configurations")]
    public string[]? SegmentDeliveryConfigurations { get; set; }

    [CommandSwitch("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}