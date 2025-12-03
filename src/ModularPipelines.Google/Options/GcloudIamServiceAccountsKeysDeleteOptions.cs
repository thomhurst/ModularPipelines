using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iam", "service-accounts", "keys", "delete")]
public record GcloudIamServiceAccountsKeysDeleteOptions(
[property: CliArgument] string KeyId,
[property: CliOption("--iam-account")] string IamAccount
) : GcloudOptions;