using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("secrets", "set-iam-policy")]
public record GcloudSecretsSetIamPolicyOptions(
[property: CliArgument] string Secret,
[property: CliArgument] string PolicyFile
) : GcloudOptions;