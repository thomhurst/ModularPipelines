using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("edgeorder", "list-family")]
public record AzEdgeorderListFamilyOptions(
[property: CliOption("--filterable-properties")] string FilterableProperties
) : AzOptions
{
    [CliOption("--expand")]
    public string? Expand { get; set; }

    [CliOption("--location-placement-id")]
    public string? LocationPlacementId { get; set; }

    [CliOption("--quota-id")]
    public string? QuotaId { get; set; }

    [CliOption("--registered-features")]
    public string? RegisteredFeatures { get; set; }

    [CliOption("--skip-token")]
    public string? SkipToken { get; set; }
}