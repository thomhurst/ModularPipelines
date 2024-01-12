using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("resource-manager", "folders", "delete")]
public record GcloudResourceManagerFoldersDeleteOptions(
[property: PositionalArgument] string FolderId
) : GcloudOptions;