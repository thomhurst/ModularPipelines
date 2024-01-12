using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("deploy", "targets", "get-iam-policy")]
public record GcloudDeployTargetsGetIamPolicyOptions(
[property: PositionalArgument] string Target,
[property: PositionalArgument] string Region
) : GcloudOptions;