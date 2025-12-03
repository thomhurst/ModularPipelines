using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("keyvault", "certificate", "issuer", "list")]
public record AzKeyvaultCertificateIssuerListOptions(
[property: CliOption("--vault-name")] string VaultName
) : AzOptions;