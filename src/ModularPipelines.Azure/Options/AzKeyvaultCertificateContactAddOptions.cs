using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("keyvault", "certificate", "contact", "add")]
public record AzKeyvaultCertificateContactAddOptions(
[property: CliOption("--email")] string Email,
[property: CliOption("--vault-name")] string VaultName
) : AzOptions
{
    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--phone")]
    public string? Phone { get; set; }
}