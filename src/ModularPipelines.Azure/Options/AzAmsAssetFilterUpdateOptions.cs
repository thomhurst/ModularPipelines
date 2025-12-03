using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ams", "asset-filter", "update")]
public record AzAmsAssetFilterUpdateOptions : AzOptions
{
    [CliOption("--account-name")]
    public int? AccountName { get; set; }

    [CliOption("--add")]
    public string? Add { get; set; }

    [CliOption("--asset-name")]
    public string? AssetName { get; set; }

    [CliOption("--end-timestamp")]
    public string? EndTimestamp { get; set; }

    [CliOption("--first-quality")]
    public string? FirstQuality { get; set; }

    [CliFlag("--force-end-timestamp")]
    public bool? ForceEndTimestamp { get; set; }

    [CliFlag("--force-string")]
    public bool? ForceString { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--live-backoff-duration")]
    public string? LiveBackoffDuration { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--presentation-window-duration")]
    public string? PresentationWindowDuration { get; set; }

    [CliOption("--remove")]
    public string? Remove { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--set")]
    public string? Set { get; set; }

    [CliOption("--start-timestamp")]
    public string? StartTimestamp { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--timescale")]
    public string? Timescale { get; set; }

    [CliOption("--tracks")]
    public string? Tracks { get; set; }
}