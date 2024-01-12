using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("projects", "get-iam-policy")]
public record GcloudProjectsGetIamPolicyOptions(
[property: PositionalArgument] string ProjectIdOrNumber
) : GcloudOptions;