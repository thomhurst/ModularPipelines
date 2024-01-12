using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iam", "service-accounts", "keys", "enable")]
public record GcloudIamServiceAccountsKeysEnableOptions(
[property: PositionalArgument] string IamKey,
[property: PositionalArgument] string IamAccount
) : GcloudOptions;