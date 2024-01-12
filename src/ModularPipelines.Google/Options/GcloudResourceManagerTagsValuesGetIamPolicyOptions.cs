using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("resource-manager", "tags", "values", "get-iam-policy")]
public record GcloudResourceManagerTagsValuesGetIamPolicyOptions(
[property: PositionalArgument] string ResourceName
) : GcloudOptions;