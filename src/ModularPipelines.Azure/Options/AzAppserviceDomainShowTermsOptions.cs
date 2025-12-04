using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("appservice", "domain", "show-terms")]
public record AzAppserviceDomainShowTermsOptions(
[property: CliOption("--hostname")] string Hostname
) : AzOptions;