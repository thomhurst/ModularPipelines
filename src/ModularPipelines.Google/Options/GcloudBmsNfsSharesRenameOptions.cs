using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("bms", "nfs-shares", "rename")]
public record GcloudBmsNfsSharesRenameOptions(
[property: PositionalArgument] string NfsShare,
[property: PositionalArgument] string Region,
[property: CommandSwitch("--new-name")] string NewName
) : GcloudOptions;