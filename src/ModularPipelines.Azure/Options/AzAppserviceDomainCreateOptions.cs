using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("appservice", "domain", "create")]
public record AzAppserviceDomainCreateOptions(
[property: CliOption("--contact-info")] string ContactInfo,
[property: CliOption("--hostname")] string Hostname,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliFlag("--accept-terms")]
    public bool? AcceptTerms { get; set; }

    [CliFlag("--auto-renew")]
    public bool? AutoRenew { get; set; }

    [CliFlag("--dryrun")]
    public bool? Dryrun { get; set; }

    [CliFlag("--privacy")]
    public bool? Privacy { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }
}