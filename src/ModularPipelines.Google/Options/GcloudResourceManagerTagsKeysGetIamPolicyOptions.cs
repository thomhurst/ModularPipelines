using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("resource-manager", "tags", "keys", "get-iam-policy")]
public record GcloudResourceManagerTagsKeysGetIamPolicyOptions(
[property: PositionalArgument] string ResourceName
) : GcloudOptions;