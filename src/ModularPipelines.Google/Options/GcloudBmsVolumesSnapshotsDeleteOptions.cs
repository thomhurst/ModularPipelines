using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("bms", "volumes", "snapshots", "delete")]
public record GcloudBmsVolumesSnapshotsDeleteOptions(
[property: CliArgument] string Snapshot,
[property: CliArgument] string Region,
[property: CliArgument] string Volume
) : GcloudOptions;