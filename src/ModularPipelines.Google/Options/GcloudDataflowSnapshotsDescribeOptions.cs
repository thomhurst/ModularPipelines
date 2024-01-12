using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dataflow", "snapshots", "describe")]
public record GcloudDataflowSnapshotsDescribeOptions(
[property: PositionalArgument] string SnapshotId,
[property: CommandSwitch("--region")] string Region
) : GcloudOptions;