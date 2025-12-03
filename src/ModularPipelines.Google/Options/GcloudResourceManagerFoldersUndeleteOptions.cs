using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("resource-manager", "folders", "undelete")]
public record GcloudResourceManagerFoldersUndeleteOptions(
[property: CliArgument] string FolderId
) : GcloudOptions;