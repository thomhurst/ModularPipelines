using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("appservice", "domain", "create")]
public record AzAppserviceDomainCreateOptions(
[property: CommandSwitch("--contact-info")] string ContactInfo,
[property: CommandSwitch("--hostname")] string Hostname,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [BooleanCommandSwitch("--accept-terms")]
    public bool? AcceptTerms { get; set; }

    [BooleanCommandSwitch("--auto-renew")]
    public bool? AutoRenew { get; set; }

    [BooleanCommandSwitch("--dryrun")]
    public bool? Dryrun { get; set; }

    [BooleanCommandSwitch("--privacy")]
    public bool? Privacy { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }
}

