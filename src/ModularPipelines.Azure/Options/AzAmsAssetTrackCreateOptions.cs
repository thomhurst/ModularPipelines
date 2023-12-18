using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ams", "asset-track", "create")]
public record AzAmsAssetTrackCreateOptions(
[property: CommandSwitch("--account-name")] int AccountName,
[property: CommandSwitch("--asset-name")] string AssetName,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--track-name")] string TrackName,
[property: CommandSwitch("--track-type")] string TrackType
) : AzOptions
{
    [CommandSwitch("--display-name")]
    public string? DisplayName { get; set; }

    [CommandSwitch("--file-name")]
    public string? FileName { get; set; }

    [CommandSwitch("--language-code")]
    public string? LanguageCode { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--player-visibility")]
    public string? PlayerVisibility { get; set; }
}