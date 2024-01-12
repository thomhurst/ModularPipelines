using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("netapp", "volumes", "snapshots", "delete")]
public record GcloudNetappVolumesSnapshotsDeleteOptions(
[property: PositionalArgument] string Snapshot,
[property: PositionalArgument] string Location
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--volume")]
    public string? Volume { get; set; }
}