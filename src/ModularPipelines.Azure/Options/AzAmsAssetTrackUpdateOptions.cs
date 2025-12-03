using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ams", "asset-track", "update")]
public record AzAmsAssetTrackUpdateOptions(
[property: CliOption("--asset-name")] string AssetName,
[property: CliOption("--track-name")] string TrackName
) : AzOptions
{
    [CliOption("--account-name")]
    public int? AccountName { get; set; }

    [CliOption("--display-name")]
    public string? DisplayName { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--language-code")]
    public string? LanguageCode { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--player-visibility")]
    public string? PlayerVisibility { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}