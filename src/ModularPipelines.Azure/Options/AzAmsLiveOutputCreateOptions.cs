using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("ams", "live-output", "create")]
public record AzAmsLiveOutputCreateOptions(
[property: CliOption("--account-name")] int AccountName,
[property: CliOption("--archive-window-length")] string ArchiveWindowLength,
[property: CliOption("--asset-name")] string AssetName,
[property: CliOption("--live-event-name")] string LiveEventName,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--fragments-per-ts-segment")]
    public string? FragmentsPerTsSegment { get; set; }

    [CliOption("--manifest-name")]
    public string? ManifestName { get; set; }

    [CliOption("--output-snap-time")]
    public string? OutputSnapTime { get; set; }
}