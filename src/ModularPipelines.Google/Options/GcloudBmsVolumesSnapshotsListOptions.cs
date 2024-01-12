using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("bms", "volumes", "snapshots", "list")]
public record GcloudBmsVolumesSnapshotsListOptions(
[property: CommandSwitch("--volume")] string Volume,
[property: CommandSwitch("--region")] string Region
) : GcloudOptions;