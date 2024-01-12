using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("deploy", "targets", "set-iam-policy")]
public record GcloudDeployTargetsSetIamPolicyOptions(
[property: PositionalArgument] string Target,
[property: PositionalArgument] string Region,
[property: PositionalArgument] string PolicyFile
) : GcloudOptions;