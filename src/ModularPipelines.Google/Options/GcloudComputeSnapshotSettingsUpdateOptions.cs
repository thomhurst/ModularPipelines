using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "snapshot-settings", "update")]
public record GcloudComputeSnapshotSettingsUpdateOptions : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--storage-location-names")]
    public string[]? StorageLocationNames { get; set; }

    [CommandSwitch("--storage-location-policy")]
    public string? StorageLocationPolicy { get; set; }
}