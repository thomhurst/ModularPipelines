using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("keyvault", "certificate", "issuer", "admin", "list")]
public record AzKeyvaultCertificateIssuerAdminListOptions(
[property: CommandSwitch("--issuer-name")] string IssuerName,
[property: CommandSwitch("--vault-name")] string VaultName
) : AzOptions;