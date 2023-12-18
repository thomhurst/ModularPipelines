using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("keyvault", "certificate", "contact", "delete")]
public record AzKeyvaultCertificateContactDeleteOptions(
[property: CommandSwitch("--email")] string Email,
[property: CommandSwitch("--vault-name")] string VaultName
) : AzOptions
{
}