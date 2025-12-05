using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("storage", "account", "or-policy", "show")]
public record AzStorageAccountOrPolicyShowOptions(
[property: CliOption("--account-name")] int AccountName,
[property: CliOption("--policy-id")] string PolicyId
) : AzOptions
{
    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }
}