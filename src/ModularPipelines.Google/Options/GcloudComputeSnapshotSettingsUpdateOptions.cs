using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "snapshot-settings", "update")]
public record GcloudComputeSnapshotSettingsUpdateOptions : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--storage-location-names")]
    public string[]? StorageLocationNames { get; set; }

    [CliOption("--storage-location-policy")]
    public string? StorageLocationPolicy { get; set; }
}