using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ams", "asset-filter", "create")]
public record AzAmsAssetFilterCreateOptions(
[property: CliOption("--account-name")] int AccountName,
[property: CliOption("--asset-name")] string AssetName,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--end-timestamp")]
    public string? EndTimestamp { get; set; }

    [CliOption("--first-quality")]
    public string? FirstQuality { get; set; }

    [CliFlag("--force-end-timestamp")]
    public bool? ForceEndTimestamp { get; set; }

    [CliOption("--live-backoff-duration")]
    public string? LiveBackoffDuration { get; set; }

    [CliOption("--presentation-window-duration")]
    public string? PresentationWindowDuration { get; set; }

    [CliOption("--start-timestamp")]
    public string? StartTimestamp { get; set; }

    [CliOption("--timescale")]
    public string? Timescale { get; set; }

    [CliOption("--tracks")]
    public string? Tracks { get; set; }
}