using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("ams", "asset-track", "create")]
public record AzAmsAssetTrackCreateOptions(
[property: CliOption("--account-name")] int AccountName,
[property: CliOption("--asset-name")] string AssetName,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--track-name")] string TrackName,
[property: CliOption("--track-type")] string TrackType
) : AzOptions
{
    [CliOption("--display-name")]
    public string? DisplayName { get; set; }

    [CliOption("--file-name")]
    public string? FileName { get; set; }

    [CliOption("--language-code")]
    public string? LanguageCode { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--player-visibility")]
    public string? PlayerVisibility { get; set; }
}