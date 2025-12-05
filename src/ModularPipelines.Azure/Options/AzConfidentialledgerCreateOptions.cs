using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("confidentialledger", "create")]
public record AzConfidentialledgerCreateOptions(
[property: CliOption("--ledger-name")] string LedgerName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--aad-based-security-principals")]
    public string? AadBasedSecurityPrincipals { get; set; }

    [CliOption("--cert-based-security-principals")]
    public string? CertBasedSecurityPrincipals { get; set; }

    [CliOption("--ledger-type")]
    public string? LedgerType { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }
}