using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("resource-manager", "tags", "values", "get-iam-policy")]
public record GcloudResourceManagerTagsValuesGetIamPolicyOptions(
[property: CliArgument] string ResourceName
) : GcloudOptions;