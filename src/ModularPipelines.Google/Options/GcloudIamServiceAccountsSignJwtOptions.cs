using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iam", "service-accounts", "sign-jwt")]
public record GcloudIamServiceAccountsSignJwtOptions(
[property: PositionalArgument] string InputFile,
[property: PositionalArgument] string OutputFile,
[property: CommandSwitch("--iam-account")] string IamAccount
) : GcloudOptions;