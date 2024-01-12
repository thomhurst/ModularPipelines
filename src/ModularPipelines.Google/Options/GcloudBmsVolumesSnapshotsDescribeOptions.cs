using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("bms", "volumes", "snapshots", "describe")]
public record GcloudBmsVolumesSnapshotsDescribeOptions(
[property: PositionalArgument] string Snapshot,
[property: PositionalArgument] string Region,
[property: PositionalArgument] string Volume
) : GcloudOptions;