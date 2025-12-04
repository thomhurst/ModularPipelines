using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("keyvault", "security-domain", "init-recovery")]
public record AzKeyvaultSecurityDomainInitRecoveryOptions(
[property: CliOption("--sd-exchange-key")] string SdExchangeKey
) : AzOptions
{
    [CliOption("--hsm-name")]
    public string? HsmName { get; set; }

    [CliOption("--id")]
    public string? Id { get; set; }
}