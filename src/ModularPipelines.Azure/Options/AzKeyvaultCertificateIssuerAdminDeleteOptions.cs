using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("keyvault", "certificate", "issuer", "admin", "delete")]
public record AzKeyvaultCertificateIssuerAdminDeleteOptions(
[property: CommandSwitch("--email")] string Email,
[property: CommandSwitch("--issuer-name")] string IssuerName,
[property: CommandSwitch("--vault-name")] string VaultName
) : AzOptions
{
}

