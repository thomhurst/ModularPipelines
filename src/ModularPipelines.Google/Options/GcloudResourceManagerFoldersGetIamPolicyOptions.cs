using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("resource-manager", "folders", "get-iam-policy")]
public record GcloudResourceManagerFoldersGetIamPolicyOptions(
[property: CliArgument] string FolderId
) : GcloudOptions;