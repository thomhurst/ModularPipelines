using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("bms", "instances", "rename")]
public record GcloudBmsInstancesRenameOptions(
[property: PositionalArgument] string Instance,
[property: PositionalArgument] string Region,
[property: CommandSwitch("--new-name")] string NewName
) : GcloudOptions;