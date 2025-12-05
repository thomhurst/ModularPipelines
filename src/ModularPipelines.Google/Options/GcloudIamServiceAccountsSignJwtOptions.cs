using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iam", "service-accounts", "sign-jwt")]
public record GcloudIamServiceAccountsSignJwtOptions(
[property: CliArgument] string InputFile,
[property: CliArgument] string OutputFile,
[property: CliOption("--iam-account")] string IamAccount
) : GcloudOptions;