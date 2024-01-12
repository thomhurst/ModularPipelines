using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("bms", "volumes", "snapshot")]
public record GcloudBmsVolumesSnapshotOptions(
[property: PositionalArgument] string Volume,
[property: PositionalArgument] string Region,
[property: CommandSwitch("--description")] string Description,
[property: CommandSwitch("--snapshot-name")] string SnapshotName
) : GcloudOptions;