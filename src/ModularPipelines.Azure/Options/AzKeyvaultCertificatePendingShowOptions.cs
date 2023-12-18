using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("keyvault", "certificate", "pending", "show")]
public record AzKeyvaultCertificatePendingShowOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--vault-name")] string VaultName
) : AzOptions
{
}