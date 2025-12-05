using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("edge-cloud", "container", "clusters", "update")]
public record GcloudEdgeCloudContainerClustersUpdateOptions(
[property: CliArgument] string Cluster,
[property: CliArgument] string Location
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliFlag("--clear-maintenance-window")]
    public bool? ClearMaintenanceWindow { get; set; }

    [CliOption("--maintenance-window-end")]
    public string? MaintenanceWindowEnd { get; set; }

    [CliOption("--maintenance-window-recurrence")]
    public string? MaintenanceWindowRecurrence { get; set; }

    [CliOption("--maintenance-window-start")]
    public string? MaintenanceWindowStart { get; set; }
}