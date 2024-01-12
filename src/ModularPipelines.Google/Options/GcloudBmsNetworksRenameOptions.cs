using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("bms", "networks", "rename")]
public record GcloudBmsNetworksRenameOptions(
[property: PositionalArgument] string Network,
[property: PositionalArgument] string Region,
[property: CommandSwitch("--new-name")] string NewName
) : GcloudOptions;