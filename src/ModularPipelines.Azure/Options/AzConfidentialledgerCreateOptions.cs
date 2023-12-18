using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("confidentialledger", "create")]
public record AzConfidentialledgerCreateOptions(
[property: CommandSwitch("--ledger-name")] string LedgerName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--aad-based-security-principals")]
    public string? AadBasedSecurityPrincipals { get; set; }

    [CommandSwitch("--cert-based-security-principals")]
    public string? CertBasedSecurityPrincipals { get; set; }

    [CommandSwitch("--ledger-type")]
    public string? LedgerType { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }
}