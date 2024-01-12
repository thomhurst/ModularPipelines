using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("secrets", "get-iam-policy")]
public record GcloudSecretsGetIamPolicyOptions(
[property: PositionalArgument] string Secret
) : GcloudOptions;