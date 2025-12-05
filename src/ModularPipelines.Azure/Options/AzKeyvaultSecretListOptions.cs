using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("keyvault", "secret", "list")]
public record AzKeyvaultSecretListOptions : AzOptions
{
    [CliOption("--id")]
    public string? Id { get; set; }

    [CliFlag("--include-managed")]
    public bool? IncludeManaged { get; set; }

    [CliOption("--maxresults")]
    public string? Maxresults { get; set; }

    [CliOption("--vault-name")]
    public string? VaultName { get; set; }
}