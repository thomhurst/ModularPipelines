using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("storage", "account", "or-policy", "list")]
public record AzStorageAccountOrPolicyListOptions(
[property: CliOption("--account-name")] int AccountName
) : AzOptions
{
    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }
}