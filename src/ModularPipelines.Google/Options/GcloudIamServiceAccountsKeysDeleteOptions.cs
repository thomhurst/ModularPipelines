using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iam", "service-accounts", "keys", "delete")]
public record GcloudIamServiceAccountsKeysDeleteOptions(
[property: PositionalArgument] string KeyId,
[property: CommandSwitch("--iam-account")] string IamAccount
) : GcloudOptions;