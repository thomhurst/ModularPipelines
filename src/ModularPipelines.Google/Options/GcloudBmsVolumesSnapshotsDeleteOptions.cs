using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("bms", "volumes", "snapshots", "delete")]
public record GcloudBmsVolumesSnapshotsDeleteOptions(
[property: PositionalArgument] string Snapshot,
[property: PositionalArgument] string Region,
[property: PositionalArgument] string Volume
) : GcloudOptions;