using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sentinel", "enrichment", "ip-geodata", "show")]
public record AzSentinelEnrichmentIpGeodataShowOptions(
[property: CliOption("--ip-address")] string IpAddress,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;