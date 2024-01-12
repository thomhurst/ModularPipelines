using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("resource-manager", "tags", "values", "set-iam-policy")]
public record GcloudResourceManagerTagsValuesSetIamPolicyOptions(
[property: PositionalArgument] string ResourceName,
[property: PositionalArgument] string PolicyFile
) : GcloudOptions;