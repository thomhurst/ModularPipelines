using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("keyvault", "secret", "list-deleted")]
public record AzKeyvaultSecretListDeletedOptions : AzOptions
{
    [CliOption("--id")]
    public string? Id { get; set; }

    [CliOption("--maxresults")]
    public string? Maxresults { get; set; }

    [CliOption("--vault-name")]
    public string? VaultName { get; set; }
}