using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("keyvault", "certificate", "pending", "delete")]
public record AzKeyvaultCertificatePendingDeleteOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--vault-name")] string VaultName
) : AzOptions;