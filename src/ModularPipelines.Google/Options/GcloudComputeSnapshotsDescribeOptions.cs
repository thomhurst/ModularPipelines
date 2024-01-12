using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "snapshots", "describe")]
public record GcloudComputeSnapshotsDescribeOptions(
[property: PositionalArgument] string SnapshotName
) : GcloudOptions;