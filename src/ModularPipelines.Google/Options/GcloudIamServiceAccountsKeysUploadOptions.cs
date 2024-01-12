using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iam", "service-accounts", "keys", "upload")]
public record GcloudIamServiceAccountsKeysUploadOptions(
[property: PositionalArgument] string PublicKeyFile,
[property: CommandSwitch("--iam-account")] string IamAccount
) : GcloudOptions;