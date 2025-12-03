using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "snapshots", "describe")]
public record GcloudComputeSnapshotsDescribeOptions(
[property: CliArgument] string SnapshotName
) : GcloudOptions;