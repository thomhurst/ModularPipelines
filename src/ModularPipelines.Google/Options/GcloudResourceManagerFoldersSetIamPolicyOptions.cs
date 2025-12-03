using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("resource-manager", "folders", "set-iam-policy")]
public record GcloudResourceManagerFoldersSetIamPolicyOptions(
[property: CliArgument] string FolderId,
[property: CliArgument] string PolicyFile
) : GcloudOptions;