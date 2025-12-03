using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("mediatailor", "update-source-location")]
public record AwsMediatailorUpdateSourceLocationOptions(
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

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}