using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("storage", "account", "keys", "list")]
public record AzStorageAccountKeysListOptions(
[property: CliOption("--account-name")] int AccountName
) : AzOptions
{
    [CliOption("--expand-key-type")]
    public string? ExpandKeyType { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }
}