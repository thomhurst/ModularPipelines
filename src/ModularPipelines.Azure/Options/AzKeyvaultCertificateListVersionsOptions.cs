using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("keyvault", "certificate", "list-versions")]
public record AzKeyvaultCertificateListVersionsOptions : AzOptions
{
    [CliOption("--id")]
    public string? Id { get; set; }

    [CliOption("--maxresults")]
    public string? Maxresults { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--vault-name")]
    public string? VaultName { get; set; }
}