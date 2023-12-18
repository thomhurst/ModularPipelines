using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("appservice", "domain", "show-terms")]
public record AzAppserviceDomainShowTermsOptions(
[property: CommandSwitch("--hostname")] string Hostname
) : AzOptions
{
}