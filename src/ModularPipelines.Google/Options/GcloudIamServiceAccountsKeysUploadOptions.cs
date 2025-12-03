using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iam", "service-accounts", "keys", "upload")]
public record GcloudIamServiceAccountsKeysUploadOptions(
[property: CliArgument] string PublicKeyFile,
[property: CliOption("--iam-account")] string IamAccount
) : GcloudOptions;