using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iam", "service-accounts", "keys", "list")]
public record GcloudIamServiceAccountsKeysListOptions(
[property: CommandSwitch("--iam-account")] string IamAccount
) : GcloudOptions
{
    [CommandSwitch("--created-before")]
    public string? CreatedBefore { get; set; }

    [CommandSwitch("--managed-by")]
    public string? ManagedBy { get; set; }
}