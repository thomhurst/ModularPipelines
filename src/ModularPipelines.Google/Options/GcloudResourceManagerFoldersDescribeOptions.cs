using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("resource-manager", "folders", "describe")]
public record GcloudResourceManagerFoldersDescribeOptions(
[property: PositionalArgument] string FolderId
) : GcloudOptions;