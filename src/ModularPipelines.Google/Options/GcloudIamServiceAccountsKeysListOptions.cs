using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iam", "service-accounts", "keys", "list")]
public record GcloudIamServiceAccountsKeysListOptions(
[property: CliOption("--iam-account")] string IamAccount
) : GcloudOptions
{
    [CliOption("--created-before")]
    public string? CreatedBefore { get; set; }

    [CliOption("--managed-by")]
    public string? ManagedBy { get; set; }
}