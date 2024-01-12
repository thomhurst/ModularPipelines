using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iam", "service-accounts", "keys", "disable")]
public record GcloudIamServiceAccountsKeysDisableOptions(
[property: PositionalArgument] string IamKey,
[property: PositionalArgument] string IamAccount
) : GcloudOptions;