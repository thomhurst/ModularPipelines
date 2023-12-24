using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("outposts", "update-site-rack-physical-properties")]
public record AwsOutpostsUpdateSiteRackPhysicalPropertiesOptions(
[property: CommandSwitch("--site-id")] string SiteId
) : AwsOptions
{
    [CommandSwitch("--power-draw-kva")]
    public string? PowerDrawKva { get; set; }

    [CommandSwitch("--power-phase")]
    public string? PowerPhase { get; set; }

    [CommandSwitch("--power-connector")]
    public string? PowerConnector { get; set; }

    [CommandSwitch("--power-feed-drop")]
    public string? PowerFeedDrop { get; set; }

    [CommandSwitch("--uplink-gbps")]
    public string? UplinkGbps { get; set; }

    [CommandSwitch("--uplink-count")]
    public string? UplinkCount { get; set; }

    [CommandSwitch("--fiber-optic-cable-type")]
    public string? FiberOpticCableType { get; set; }

    [CommandSwitch("--optical-standard")]
    public string? OpticalStandard { get; set; }

    [CommandSwitch("--maximum-supported-weight-lbs")]
    public string? MaximumSupportedWeightLbs { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}