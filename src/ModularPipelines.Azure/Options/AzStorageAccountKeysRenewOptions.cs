using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("storage", "account", "keys", "renew")]
public record AzStorageAccountKeysRenewOptions(
[property: CliOption("--account-name")] int AccountName,
[property: CliOption("--key")] string Key
) : AzOptions
{
    [CliOption("--key-type")]
    public string? KeyType { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }
}