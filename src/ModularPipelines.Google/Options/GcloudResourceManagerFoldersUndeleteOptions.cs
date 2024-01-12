using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("resource-manager", "folders", "undelete")]
public record GcloudResourceManagerFoldersUndeleteOptions(
[property: PositionalArgument] string FolderId
) : GcloudOptions;