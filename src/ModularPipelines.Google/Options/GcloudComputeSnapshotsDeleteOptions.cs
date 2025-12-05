using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "snapshots", "delete")]
public record GcloudComputeSnapshotsDeleteOptions(
[property: CliArgument] string SnapshotName
) : GcloudOptions;