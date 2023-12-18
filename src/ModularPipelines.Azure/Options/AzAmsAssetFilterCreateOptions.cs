using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ams", "asset-filter", "create")]
public record AzAmsAssetFilterCreateOptions(
[property: CommandSwitch("--account-name")] int AccountName,
[property: CommandSwitch("--asset-name")] string AssetName,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--end-timestamp")]
    public string? EndTimestamp { get; set; }

    [CommandSwitch("--first-quality")]
    public string? FirstQuality { get; set; }

    [BooleanCommandSwitch("--force-end-timestamp")]
    public bool? ForceEndTimestamp { get; set; }

    [CommandSwitch("--live-backoff-duration")]
    public string? LiveBackoffDuration { get; set; }

    [CommandSwitch("--presentation-window-duration")]
    public string? PresentationWindowDuration { get; set; }

    [CommandSwitch("--start-timestamp")]
    public string? StartTimestamp { get; set; }

    [CommandSwitch("--timescale")]
    public string? Timescale { get; set; }

    [CommandSwitch("--tracks")]
    public string? Tracks { get; set; }
}