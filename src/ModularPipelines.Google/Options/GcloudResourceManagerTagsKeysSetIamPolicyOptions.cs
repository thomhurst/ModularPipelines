using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("resource-manager", "tags", "keys", "set-iam-policy")]
public record GcloudResourceManagerTagsKeysSetIamPolicyOptions(
[property: PositionalArgument] string ResourceName,
[property: PositionalArgument] string PolicyFile
) : GcloudOptions;