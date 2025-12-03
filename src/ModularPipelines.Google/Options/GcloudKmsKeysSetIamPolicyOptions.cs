using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("kms", "keys", "set-iam-policy")]
public record GcloudKmsKeysSetIamPolicyOptions(
[property: CliArgument] string Key,
[property: CliArgument] string Keyring,
[property: CliArgument] string Location,
[property: CliArgument] string PolicyFile
) : GcloudOptions;