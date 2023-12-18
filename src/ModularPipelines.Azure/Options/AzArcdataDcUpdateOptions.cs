using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("arcdata", "dc", "update")]
public record AzArcdataDcUpdateOptions(
[property: CommandSwitch("--path")] string Path
) : AzOptions
{
    [BooleanCommandSwitch("--auto-upload-logs")]
    public bool? AutoUploadLogs { get; set; }

    [BooleanCommandSwitch("--auto-upload-metrics")]
    public bool? AutoUploadMetrics { get; set; }

    [CommandSwitch("--desired-version")]
    public string? DesiredVersion { get; set; }

    [CommandSwitch("--k8s-namespace")]
    public string? K8sNamespace { get; set; }

    [CommandSwitch("--maintenance-duration")]
    public string? MaintenanceDuration { get; set; }

    [BooleanCommandSwitch("--maintenance-enabled")]
    public bool? MaintenanceEnabled { get; set; }

    [CommandSwitch("--maintenance-recurrence")]
    public string? MaintenanceRecurrence { get; set; }

    [CommandSwitch("--maintenance-start")]
    public string? MaintenanceStart { get; set; }

    [CommandSwitch("--maintenance-time-zone")]
    public string? MaintenanceTimeZone { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--use-k8s")]
    public string? UseK8s { get; set; }
}