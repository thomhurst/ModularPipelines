using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("keyvault", "certificate", "list-deleted")]
public record AzKeyvaultCertificateListDeletedOptions : AzOptions
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