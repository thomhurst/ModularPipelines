using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("edgeorder", "list-config")]
public record AzEdgeorderListConfigOptions(
[property: CommandSwitch("--configuration-filters")] string ConfigurationFilters
) : AzOptions
{
    [CommandSwitch("--location-placement-id")]
    public string? LocationPlacementId { get; set; }

    [CommandSwitch("--quota-id")]
    public string? QuotaId { get; set; }

    [CommandSwitch("--registered-features")]
    public string? RegisteredFeatures { get; set; }

    [CommandSwitch("--skip-token")]
    public string? SkipToken { get; set; }
}

