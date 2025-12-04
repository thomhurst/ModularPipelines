using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("edgeorder", "list-config")]
public record AzEdgeorderListConfigOptions(
[property: CliOption("--configuration-filters")] string ConfigurationFilters
) : AzOptions
{
    [CliOption("--location-placement-id")]
    public string? LocationPlacementId { get; set; }

    [CliOption("--quota-id")]
    public string? QuotaId { get; set; }

    [CliOption("--registered-features")]
    public string? RegisteredFeatures { get; set; }

    [CliOption("--skip-token")]
    public string? SkipToken { get; set; }
}