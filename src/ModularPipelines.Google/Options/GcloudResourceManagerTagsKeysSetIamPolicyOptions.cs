using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("resource-manager", "tags", "keys", "set-iam-policy")]
public record GcloudResourceManagerTagsKeysSetIamPolicyOptions(
[property: CliArgument] string ResourceName,
[property: CliArgument] string PolicyFile
) : GcloudOptions;