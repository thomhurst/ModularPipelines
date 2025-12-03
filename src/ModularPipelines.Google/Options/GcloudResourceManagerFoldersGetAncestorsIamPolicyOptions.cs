using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("resource-manager", "folders", "get-ancestors-iam-policy")]
public record GcloudResourceManagerFoldersGetAncestorsIamPolicyOptions(
[property: CliArgument] string FolderId
) : GcloudOptions;