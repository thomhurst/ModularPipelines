using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("secrets", "set-iam-policy")]
public record GcloudSecretsSetIamPolicyOptions(
[property: PositionalArgument] string Secret,
[property: PositionalArgument] string PolicyFile
) : GcloudOptions;