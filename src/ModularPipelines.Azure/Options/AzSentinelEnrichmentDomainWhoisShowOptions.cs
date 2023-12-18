using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sentinel", "enrichment", "domain-whois", "show")]
public record AzSentinelEnrichmentDomainWhoisShowOptions(
[property: CommandSwitch("--domain")] string Domain,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions;