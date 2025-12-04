using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("keyvault", "certificate", "restore")]
public record AzKeyvaultCertificateRestoreOptions(
[property: CliOption("--file")] string File
) : AzOptions
{
    [CliOption("--vault-name")]
    public string? VaultName { get; set; }
}