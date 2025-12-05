using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iam", "service-accounts", "keys", "create")]
public record GcloudIamServiceAccountsKeysCreateOptions(
[property: CliArgument] string OutputFile,
[property: CliOption("--iam-account")] string IamAccount
) : GcloudOptions
{
    [CliOption("--key-file-type")]
    public string? KeyFileType { get; set; }
}