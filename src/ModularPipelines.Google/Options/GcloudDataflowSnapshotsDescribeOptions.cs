using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dataflow", "snapshots", "describe")]
public record GcloudDataflowSnapshotsDescribeOptions(
[property: CliArgument] string SnapshotId,
[property: CliOption("--region")] string Region
) : GcloudOptions;