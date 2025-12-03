using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sentinel", "enrichment", "domain-whois", "show")]
public record AzSentinelEnrichmentDomainWhoisShowOptions(
[property: CliOption("--domain")] string Domain,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;