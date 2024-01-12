using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("kms", "keys", "set-iam-policy")]
public record GcloudKmsKeysSetIamPolicyOptions(
[property: PositionalArgument] string Key,
[property: PositionalArgument] string Keyring,
[property: PositionalArgument] string Location,
[property: PositionalArgument] string PolicyFile
) : GcloudOptions;