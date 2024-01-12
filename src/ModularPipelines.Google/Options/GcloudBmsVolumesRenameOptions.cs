using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("bms", "volumes", "rename")]
public record GcloudBmsVolumesRenameOptions(
[property: PositionalArgument] string Volume,
[property: PositionalArgument] string Region,
[property: CommandSwitch("--new-name")] string NewName
) : GcloudOptions;