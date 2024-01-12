using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("edge-cloud", "container", "clusters", "update")]
public record GcloudEdgeCloudContainerClustersUpdateOptions(
[property: PositionalArgument] string Cluster,
[property: PositionalArgument] string Location
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [BooleanCommandSwitch("--clear-maintenance-window")]
    public bool? ClearMaintenanceWindow { get; set; }

    [CommandSwitch("--maintenance-window-end")]
    public string? MaintenanceWindowEnd { get; set; }

    [CommandSwitch("--maintenance-window-recurrence")]
    public string? MaintenanceWindowRecurrence { get; set; }

    [CommandSwitch("--maintenance-window-start")]
    public string? MaintenanceWindowStart { get; set; }
}