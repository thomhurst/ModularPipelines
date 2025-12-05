using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("keyvault", "certificate", "issuer", "update")]
public record AzKeyvaultCertificateIssuerUpdateOptions(
[property: CliOption("--issuer-name")] string IssuerName,
[property: CliOption("--vault-name")] string VaultName
) : AzOptions
{
    [CliOption("--account-id")]
    public int? AccountId { get; set; }

    [CliFlag("--enabled")]
    public bool? Enabled { get; set; }

    [CliOption("--organization-id")]
    public string? OrganizationId { get; set; }

    [CliOption("--password")]
    public string? Password { get; set; }

    [CliOption("--provider-name")]
    public string? ProviderName { get; set; }
}