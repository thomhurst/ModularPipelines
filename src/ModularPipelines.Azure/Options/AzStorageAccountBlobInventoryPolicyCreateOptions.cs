using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("storage", "account", "blob-inventory-policy", "create")]
public record AzStorageAccountBlobInventoryPolicyCreateOptions(
[property: CliOption("--account-name")] int AccountName,
[property: CliOption("--policy")] string Policy
) : AzOptions
{
    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }
}