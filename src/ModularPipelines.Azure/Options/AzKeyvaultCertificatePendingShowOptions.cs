using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("keyvault", "certificate", "pending", "show")]
public record AzKeyvaultCertificatePendingShowOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--vault-name")] string VaultName
) : AzOptions;