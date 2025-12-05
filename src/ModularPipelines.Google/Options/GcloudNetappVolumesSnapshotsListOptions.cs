using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("netapp", "volumes", "snapshots", "list")]
public record GcloudNetappVolumesSnapshotsListOptions : GcloudOptions
{
    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--volume")]
    public string? Volume { get; set; }
}