using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "snapshots", "delete")]
public record GcloudComputeSnapshotsDeleteOptions(
[property: PositionalArgument] string SnapshotName
) : GcloudOptions;