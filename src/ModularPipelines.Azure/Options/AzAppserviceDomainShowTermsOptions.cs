using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("appservice", "domain", "show-terms")]
public record AzAppserviceDomainShowTermsOptions(
[property: CommandSwitch("--hostname")] string Hostname
) : AzOptions;