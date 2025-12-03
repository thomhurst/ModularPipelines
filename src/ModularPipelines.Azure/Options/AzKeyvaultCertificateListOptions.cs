using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("keyvault", "certificate", "list")]
public record AzKeyvaultCertificateListOptions : AzOptions
{
    [CliOption("--id")]
    public string? Id { get; set; }

    [CliFlag("--include-pending")]
    public bool? IncludePending { get; set; }

    [CliOption("--maxresults")]
    public string? Maxresults { get; set; }

    [CliOption("--vault-name")]
    public string? VaultName { get; set; }
}