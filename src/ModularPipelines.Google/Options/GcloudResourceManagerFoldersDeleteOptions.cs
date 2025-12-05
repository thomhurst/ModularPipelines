using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("resource-manager", "folders", "delete")]
public record GcloudResourceManagerFoldersDeleteOptions(
[property: CliArgument] string FolderId
) : GcloudOptions;