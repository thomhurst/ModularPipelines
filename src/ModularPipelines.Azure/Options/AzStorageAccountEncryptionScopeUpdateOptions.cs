using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("storage", "account", "encryption-scope", "update")]
public record AzStorageAccountEncryptionScopeUpdateOptions(
[property: CliOption("--account-name")] int AccountName,
[property: CliOption("--name")] string Name
) : AzOptions
{
    [CliOption("--key-source")]
    public string? KeySource { get; set; }

    [CliOption("--key-uri")]
    public string? KeyUri { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--state")]
    public string? State { get; set; }
}