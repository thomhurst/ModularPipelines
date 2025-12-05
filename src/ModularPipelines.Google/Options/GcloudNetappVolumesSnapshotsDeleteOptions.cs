using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("netapp", "volumes", "snapshots", "delete")]
public record GcloudNetappVolumesSnapshotsDeleteOptions(
[property: CliArgument] string Snapshot,
[property: CliArgument] string Location
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--volume")]
    public string? Volume { get; set; }
}