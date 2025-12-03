using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("keyvault", "certificate", "contact", "list")]
public record AzKeyvaultCertificateContactListOptions(
[property: CliOption("--vault-name")] string VaultName
) : AzOptions;