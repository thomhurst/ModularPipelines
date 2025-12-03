using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("keyvault", "secret", "restore")]
public record AzKeyvaultSecretRestoreOptions(
[property: CliOption("--file")] string File,
[property: CliOption("--vault-name")] string VaultName
) : AzOptions;