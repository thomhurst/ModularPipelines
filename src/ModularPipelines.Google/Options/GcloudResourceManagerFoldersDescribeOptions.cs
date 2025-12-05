using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("resource-manager", "folders", "describe")]
public record GcloudResourceManagerFoldersDescribeOptions(
[property: CliArgument] string FolderId
) : GcloudOptions;