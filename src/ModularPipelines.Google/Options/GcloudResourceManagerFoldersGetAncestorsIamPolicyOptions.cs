using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("resource-manager", "folders", "get-ancestors-iam-policy")]
public record GcloudResourceManagerFoldersGetAncestorsIamPolicyOptions(
[property: PositionalArgument] string FolderId
) : GcloudOptions;