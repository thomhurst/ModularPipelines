using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("outposts", "update-site-rack-physical-properties")]
public record AwsOutpostsUpdateSiteRackPhysicalPropertiesOptions(
[property: CliOption("--site-id")] string SiteId
) : AwsOptions
{
    [CliOption("--power-draw-kva")]
    public string? PowerDrawKva { get; set; }

    [CliOption("--power-phase")]
    public string? PowerPhase { get; set; }

    [CliOption("--power-connector")]
    public string? PowerConnector { get; set; }

    [CliOption("--power-feed-drop")]
    public string? PowerFeedDrop { get; set; }

    [CliOption("--uplink-gbps")]
    public string? UplinkGbps { get; set; }

    [CliOption("--uplink-count")]
    public string? UplinkCount { get; set; }

    [CliOption("--fiber-optic-cable-type")]
    public string? FiberOpticCableType { get; set; }

    [CliOption("--optical-standard")]
    public string? OpticalStandard { get; set; }

    [CliOption("--maximum-supported-weight-lbs")]
    public string? MaximumSupportedWeightLbs { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}