using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("netapp", "volumes", "snapshots", "describe")]
public record GcloudNetappVolumesSnapshotsDescribeOptions(
[property: PositionalArgument] string Snapshot,
[property: PositionalArgument] string Location
) : GcloudOptions
{
    [CommandSwitch("--volume")]
    public string? Volume { get; set; }
}