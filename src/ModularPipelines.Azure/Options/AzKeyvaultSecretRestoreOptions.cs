using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("keyvault", "secret", "restore")]
public record AzKeyvaultSecretRestoreOptions(
[property: CommandSwitch("--file")] string File,
[property: CommandSwitch("--vault-name")] string VaultName
) : AzOptions
{
}