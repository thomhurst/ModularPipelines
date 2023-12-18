using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("keyvault", "certificate", "issuer", "list")]
public record AzKeyvaultCertificateIssuerListOptions(
[property: CommandSwitch("--vault-name")] string VaultName
) : AzOptions;