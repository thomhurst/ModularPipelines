using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dataflow", "snapshots", "delete")]
public record GcloudDataflowSnapshotsDeleteOptions(
[property: PositionalArgument] string SnapshotId,
[property: CommandSwitch("--region")] string Region
) : GcloudOptions;