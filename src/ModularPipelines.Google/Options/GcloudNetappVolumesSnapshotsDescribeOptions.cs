using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("netapp", "volumes", "snapshots", "describe")]
public record GcloudNetappVolumesSnapshotsDescribeOptions(
[property: CliArgument] string Snapshot,
[property: CliArgument] string Location
) : GcloudOptions
{
    [CliOption("--volume")]
    public string? Volume { get; set; }
}