using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("arcdata", "dc", "update")]
public record AzArcdataDcUpdateOptions : AzOptions
{
    [CliFlag("--auto-upload-logs")]
    public bool? AutoUploadLogs { get; set; }

    [CliFlag("--auto-upload-metrics")]
    public bool? AutoUploadMetrics { get; set; }

    [CliOption("--desired-version")]
    public string? DesiredVersion { get; set; }

    [CliOption("--k8s-namespace")]
    public string? K8sNamespace { get; set; }

    [CliOption("--maintenance-duration")]
    public string? MaintenanceDuration { get; set; }

    [CliFlag("--maintenance-enabled")]
    public bool? MaintenanceEnabled { get; set; }

    [CliOption("--maintenance-recurrence")]
    public string? MaintenanceRecurrence { get; set; }

    [CliOption("--maintenance-start")]
    public string? MaintenanceStart { get; set; }

    [CliOption("--maintenance-time-zone")]
    public string? MaintenanceTimeZone { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliFlag("--use-k8s")]
    public bool? UseK8s { get; set; }
}