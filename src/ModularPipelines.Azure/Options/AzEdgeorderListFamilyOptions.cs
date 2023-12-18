using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("edgeorder", "list-family")]
public record AzEdgeorderListFamilyOptions(
[property: CommandSwitch("--filterable-properties")] string FilterableProperties
) : AzOptions
{
    [CommandSwitch("--expand")]
    public string? Expand { get; set; }

    [CommandSwitch("--location-placement-id")]
    public string? LocationPlacementId { get; set; }

    [CommandSwitch("--quota-id")]
    public string? QuotaId { get; set; }

    [CommandSwitch("--registered-features")]
    public string? RegisteredFeatures { get; set; }

    [CommandSwitch("--skip-token")]
    public string? SkipToken { get; set; }
}

