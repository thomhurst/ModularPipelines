using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iam", "service-accounts", "keys", "disable")]
public record GcloudIamServiceAccountsKeysDisableOptions(
[property: CliArgument] string IamKey,
[property: CliArgument] string IamAccount
) : GcloudOptions;