using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("projects", "set-iam-policy")]
public record GcloudProjectsSetIamPolicyOptions(
[property: PositionalArgument] string ProjectIdOrNumber,
[property: PositionalArgument] string PolicyFile
) : GcloudOptions;