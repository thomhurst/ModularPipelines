using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("resource-manager", "folders", "update")]
public record GcloudResourceManagerFoldersUpdateOptions(
[property: CliArgument] string FolderId,
[property: CliOption("--display-name")] string DisplayName
) : GcloudOptions;