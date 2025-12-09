using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("keyvault", "certificate", "issuer", "delete")]
public record AzKeyvaultCertificateIssuerDeleteOptions(
[property: CliOption("--issuer-name")] string IssuerName,
[property: CliOption("--vault-name")] string VaultName
) : AzOptions;