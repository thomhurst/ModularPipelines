using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("resource-manager", "tags", "values", "set-iam-policy")]
public record GcloudResourceManagerTagsValuesSetIamPolicyOptions(
[property: CliArgument] string ResourceName,
[property: CliArgument] string PolicyFile
) : GcloudOptions;