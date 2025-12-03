using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("confidentialledger", "update")]
public record AzConfidentialledgerUpdateOptions : AzOptions
{
    [CliOption("--aad-based-security-principals")]
    public string? AadBasedSecurityPrincipals { get; set; }

    [CliOption("--cert-based-security-principals")]
    public string? CertBasedSecurityPrincipals { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--ledger-name")]
    public string? LedgerName { get; set; }

    [CliOption("--ledger-type")]
    public string? LedgerType { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }
}