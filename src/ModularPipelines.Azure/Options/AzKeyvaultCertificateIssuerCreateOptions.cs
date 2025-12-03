using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("keyvault", "certificate", "issuer", "create")]
public record AzKeyvaultCertificateIssuerCreateOptions(
[property: CliOption("--issuer-name")] string IssuerName,
[property: CliOption("--provider-name")] string ProviderName,
[property: CliOption("--vault-name")] string VaultName
) : AzOptions
{
    [CliOption("--account-id")]
    public int? AccountId { get; set; }

    [CliFlag("--disabled")]
    public bool? Disabled { get; set; }

    [CliOption("--organization-id")]
    public string? OrganizationId { get; set; }

    [CliOption("--password")]
    public string? Password { get; set; }
}