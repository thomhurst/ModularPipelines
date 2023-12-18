using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("keyvault", "certificate", "issuer", "update")]
public record AzKeyvaultCertificateIssuerUpdateOptions(
[property: CommandSwitch("--issuer-name")] string IssuerName,
[property: CommandSwitch("--vault-name")] string VaultName
) : AzOptions
{
    [CommandSwitch("--account-id")]
    public int? AccountId { get; set; }

    [BooleanCommandSwitch("--enabled")]
    public bool? Enabled { get; set; }

    [CommandSwitch("--organization-id")]
    public string? OrganizationId { get; set; }

    [CommandSwitch("--password")]
    public string? Password { get; set; }

    [CommandSwitch("--provider-name")]
    public string? ProviderName { get; set; }
}