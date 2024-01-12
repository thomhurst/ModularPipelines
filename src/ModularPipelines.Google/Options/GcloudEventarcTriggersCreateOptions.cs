using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("eventarc", "triggers", "create")]
public record GcloudEventarcTriggersCreateOptions(
[property: PositionalArgument] string Trigger,
[property: PositionalArgument] string Location,
[property: CommandSwitch("--event-filters")] string[] EventFilters,
[property: CommandSwitch("--destination-gke-cluster")] string DestinationGkeCluster,
[property: CommandSwitch("--destination-gke-service")] string DestinationGkeService,
[property: CommandSwitch("--destination-gke-location")] string DestinationGkeLocation,
[property: CommandSwitch("--destination-gke-namespace")] string DestinationGkeNamespace,
[property: CommandSwitch("--destination-gke-path")] string DestinationGkePath,
[property: CommandSwitch("--destination-http-endpoint-uri")] string DestinationHttpEndpointUri,
[property: CommandSwitch("--network-attachment")] string NetworkAttachment,
[property: CommandSwitch("--destination-run-service")] string DestinationRunService,
[property: CommandSwitch("--destination-run-path")] string DestinationRunPath,
[property: CommandSwitch("--destination-run-region")] string DestinationRunRegion
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--channel")]
    public string? Channel { get; set; }

    [CommandSwitch("--event-data-content-type")]
    public string? EventDataContentType { get; set; }

    [CommandSwitch("--event-filters-path-pattern")]
    public string[]? EventFiltersPathPattern { get; set; }

    [CommandSwitch("--service-account")]
    public string? ServiceAccount { get; set; }

    [CommandSwitch("--transport-topic")]
    public string? TransportTopic { get; set; }
}