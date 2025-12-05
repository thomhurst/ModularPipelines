using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("storage", "account", "encryption-scope", "list")]
public record AzStorageAccountEncryptionScopeListOptions(
[property: CliOption("--account-name")] int AccountName
) : AzOptions
{
    [CliOption("--filter")]
    public string? Filter { get; set; }

    [CliOption("--include")]
    public string? Include { get; set; }

    [CliOption("--marker")]
    public string? Marker { get; set; }

    [CliOption("--maxpagesize")]
    public string? Maxpagesize { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }
}