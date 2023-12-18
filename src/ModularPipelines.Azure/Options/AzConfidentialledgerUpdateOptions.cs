using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("confidentialledger", "update")]
public record AzConfidentialledgerUpdateOptions : AzOptions
{
    [CommandSwitch("--aad-based-security-principals")]
    public string? AadBasedSecurityPrincipals { get; set; }

    [CommandSwitch("--cert-based-security-principals")]
    public string? CertBasedSecurityPrincipals { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--ledger-name")]
    public string? LedgerName { get; set; }

    [CommandSwitch("--ledger-type")]
    public string? LedgerType { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }
}