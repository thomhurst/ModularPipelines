using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("resource-manager", "folders", "get-iam-policy")]
public record GcloudResourceManagerFoldersGetIamPolicyOptions(
[property: PositionalArgument] string FolderId
) : GcloudOptions;