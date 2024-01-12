using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("projects", "get-ancestors-iam-policy")]
public record GcloudProjectsGetAncestorsIamPolicyOptions(
[property: PositionalArgument] string ProjectId
) : GcloudOptions;