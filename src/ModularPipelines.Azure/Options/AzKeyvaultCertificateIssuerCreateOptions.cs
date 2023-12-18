using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("keyvault", "certificate", "issuer", "create")]
public record AzKeyvaultCertificateIssuerCreateOptions(
[property: CommandSwitch("--issuer-name")] string IssuerName,
[property: CommandSwitch("--provider-name")] string ProviderName,
[property: CommandSwitch("--vault-name")] string VaultName
) : AzOptions
{
    [CommandSwitch("--account-id")]
    public int? AccountId { get; set; }

    [BooleanCommandSwitch("--disabled")]
    public bool? Disabled { get; set; }

    [CommandSwitch("--organization-id")]
    public string? OrganizationId { get; set; }

    [CommandSwitch("--password")]
    public string? Password { get; set; }
}

