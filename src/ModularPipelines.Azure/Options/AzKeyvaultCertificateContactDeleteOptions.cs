using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("keyvault", "certificate", "contact", "delete")]
public record AzKeyvaultCertificateContactDeleteOptions(
[property: CommandSwitch("--email")] string Email,
[property: CommandSwitch("--vault-name")] string VaultName
) : AzOptions;