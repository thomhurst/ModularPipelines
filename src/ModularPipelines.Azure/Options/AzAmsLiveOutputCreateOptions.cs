using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ams", "live-output", "create")]
public record AzAmsLiveOutputCreateOptions(
[property: CommandSwitch("--account-name")] int AccountName,
[property: CommandSwitch("--archive-window-length")] string ArchiveWindowLength,
[property: CommandSwitch("--asset-name")] string AssetName,
[property: CommandSwitch("--live-event-name")] string LiveEventName,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--fragments-per-ts-segment")]
    public string? FragmentsPerTsSegment { get; set; }

    [CommandSwitch("--manifest-name")]
    public string? ManifestName { get; set; }

    [CommandSwitch("--output-snap-time")]
    public string? OutputSnapTime { get; set; }
}