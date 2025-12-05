using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("keyvault", "certificate", "contact", "delete")]
public record AzKeyvaultCertificateContactDeleteOptions(
[property: CliOption("--email")] string Email,
[property: CliOption("--vault-name")] string VaultName
) : AzOptions;