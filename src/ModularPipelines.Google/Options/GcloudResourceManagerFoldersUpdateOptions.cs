using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("resource-manager", "folders", "update")]
public record GcloudResourceManagerFoldersUpdateOptions(
[property: PositionalArgument] string FolderId,
[property: CommandSwitch("--display-name")] string DisplayName
) : GcloudOptions;