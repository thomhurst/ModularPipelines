using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("eventarc", "triggers", "update")]
public record GcloudEventarcTriggersUpdateOptions(
[property: PositionalArgument] string Trigger,
[property: PositionalArgument] string Location
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--event-data-content-type")]
    public string? EventDataContentType { get; set; }

    [CommandSwitch("--event-filters")]
    public string[]? EventFilters { get; set; }

    [CommandSwitch("--event-filters-path-pattern")]
    public string[]? EventFiltersPathPattern { get; set; }

    [BooleanCommandSwitch("--clear-service-account")]
    public bool? ClearServiceAccount { get; set; }

    [CommandSwitch("--service-account")]
    public string? ServiceAccount { get; set; }

    [CommandSwitch("--destination-gke-namespace")]
    public string? DestinationGkeNamespace { get; set; }

    [CommandSwitch("--destination-gke-service")]
    public string? DestinationGkeService { get; set; }

    [BooleanCommandSwitch("--clear-destination-gke-path")]
    public bool? ClearDestinationGkePath { get; set; }

    [CommandSwitch("--destination-gke-path")]
    public string? DestinationGkePath { get; set; }

    [CommandSwitch("--destination-run-region")]
    public string? DestinationRunRegion { get; set; }

    [CommandSwitch("--destination-run-service")]
    public string? DestinationRunService { get; set; }

    [BooleanCommandSwitch("--clear-destination-run-path")]
    public bool? ClearDestinationRunPath { get; set; }

    [CommandSwitch("--destination-run-path")]
    public string? DestinationRunPath { get; set; }
}