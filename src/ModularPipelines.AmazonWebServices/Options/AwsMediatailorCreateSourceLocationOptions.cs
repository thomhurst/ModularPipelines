using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("mediatailor", "create-source-location")]
public record AwsMediatailorCreateSourceLocationOptions(
[property: CliOption("--http-configuration")] string HttpConfiguration,
[property: CliOption("--source-location-name")] string SourceLocationName
) : AwsOptions
{
    [CliOption("--access-configuration")]
    public string? AccessConfiguration { get; set; }

    [CliOption("--default-segment-delivery-configuration")]
    public string? DefaultSegmentDeliveryConfiguration { get; set; }

    [CliOption("--segment-delivery-configurations")]
    public string[]? SegmentDeliveryConfigurations { get; set; }

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}