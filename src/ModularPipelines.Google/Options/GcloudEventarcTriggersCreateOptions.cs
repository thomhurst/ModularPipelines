using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("eventarc", "triggers", "create")]
public record GcloudEventarcTriggersCreateOptions(
[property: CliArgument] string Trigger,
[property: CliArgument] string Location,
[property: CliOption("--event-filters")] string[] EventFilters,
[property: CliOption("--destination-gke-cluster")] string DestinationGkeCluster,
[property: CliOption("--destination-gke-service")] string DestinationGkeService,
[property: CliOption("--destination-gke-location")] string DestinationGkeLocation,
[property: CliOption("--destination-gke-namespace")] string DestinationGkeNamespace,
[property: CliOption("--destination-gke-path")] string DestinationGkePath,
[property: CliOption("--destination-http-endpoint-uri")] string DestinationHttpEndpointUri,
[property: CliOption("--network-attachment")] string NetworkAttachment,
[property: CliOption("--destination-run-service")] string DestinationRunService,
[property: CliOption("--destination-run-path")] string DestinationRunPath,
[property: CliOption("--destination-run-region")] string DestinationRunRegion
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--channel")]
    public string? Channel { get; set; }

    [CliOption("--event-data-content-type")]
    public string? EventDataContentType { get; set; }

    [CliOption("--event-filters-path-pattern")]
    public string[]? EventFiltersPathPattern { get; set; }

    [CliOption("--service-account")]
    public string? ServiceAccount { get; set; }

    [CliOption("--transport-topic")]
    public string? TransportTopic { get; set; }
}