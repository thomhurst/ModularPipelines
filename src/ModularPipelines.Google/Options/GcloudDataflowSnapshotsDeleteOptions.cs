using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dataflow", "snapshots", "delete")]
public record GcloudDataflowSnapshotsDeleteOptions(
[property: CliArgument] string SnapshotId,
[property: CliOption("--region")] string Region
) : GcloudOptions;