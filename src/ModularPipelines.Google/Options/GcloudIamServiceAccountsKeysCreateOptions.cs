using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iam", "service-accounts", "keys", "create")]
public record GcloudIamServiceAccountsKeysCreateOptions(
[property: PositionalArgument] string OutputFile,
[property: CommandSwitch("--iam-account")] string IamAccount
) : GcloudOptions
{
    [CommandSwitch("--key-file-type")]
    public string? KeyFileType { get; set; }
}