using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("eventarc", "triggers", "update")]
public record GcloudEventarcTriggersUpdateOptions(
[property: CliArgument] string Trigger,
[property: CliArgument] string Location
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--event-data-content-type")]
    public string? EventDataContentType { get; set; }

    [CliOption("--event-filters")]
    public string[]? EventFilters { get; set; }

    [CliOption("--event-filters-path-pattern")]
    public string[]? EventFiltersPathPattern { get; set; }

    [CliFlag("--clear-service-account")]
    public bool? ClearServiceAccount { get; set; }

    [CliOption("--service-account")]
    public string? ServiceAccount { get; set; }

    [CliOption("--destination-gke-namespace")]
    public string? DestinationGkeNamespace { get; set; }

    [CliOption("--destination-gke-service")]
    public string? DestinationGkeService { get; set; }

    [CliFlag("--clear-destination-gke-path")]
    public bool? ClearDestinationGkePath { get; set; }

    [CliOption("--destination-gke-path")]
    public string? DestinationGkePath { get; set; }

    [CliOption("--destination-run-region")]
    public string? DestinationRunRegion { get; set; }

    [CliOption("--destination-run-service")]
    public string? DestinationRunService { get; set; }

    [CliFlag("--clear-destination-run-path")]
    public bool? ClearDestinationRunPath { get; set; }

    [CliOption("--destination-run-path")]
    public string? DestinationRunPath { get; set; }
}