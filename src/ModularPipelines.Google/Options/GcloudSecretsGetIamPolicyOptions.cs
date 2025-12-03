using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("secrets", "get-iam-policy")]
public record GcloudSecretsGetIamPolicyOptions(
[property: CliArgument] string Secret
) : GcloudOptions;