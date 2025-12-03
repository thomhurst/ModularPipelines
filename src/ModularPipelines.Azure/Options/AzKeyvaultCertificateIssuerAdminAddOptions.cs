using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("keyvault", "certificate", "issuer", "admin", "add")]
public record AzKeyvaultCertificateIssuerAdminAddOptions(
[property: CliOption("--email")] string Email,
[property: CliOption("--issuer-name")] string IssuerName,
[property: CliOption("--vault-name")] string VaultName
) : AzOptions
{
    [CliOption("--first-name")]
    public string? FirstName { get; set; }

    [CliOption("--last-name")]
    public string? LastName { get; set; }

    [CliOption("--phone")]
    public string? Phone { get; set; }
}