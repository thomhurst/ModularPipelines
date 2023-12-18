using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("keyvault", "certificate", "issuer", "admin", "add")]
public record AzKeyvaultCertificateIssuerAdminAddOptions(
[property: CommandSwitch("--email")] string Email,
[property: CommandSwitch("--issuer-name")] string IssuerName,
[property: CommandSwitch("--vault-name")] string VaultName
) : AzOptions
{
    [CommandSwitch("--first-name")]
    public string? FirstName { get; set; }

    [CommandSwitch("--last-name")]
    public string? LastName { get; set; }

    [CommandSwitch("--phone")]
    public string? Phone { get; set; }
}

