using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("monitor", "log-analytics", "workspace", "data-export", "update")]
public record AzMonitorLogAnalyticsWorkspaceDataExportUpdateOptions : AzOptions
{
    [CliOption("--add")]
    public string? Add { get; set; }

    [CliOption("--data-export-name")]
    public string? DataExportName { get; set; }

    [CliOption("--destination")]
    public string? Destination { get; set; }

    [CliFlag("--enable")]
    public bool? Enable { get; set; }

    [CliOption("--event-hub-name")]
    public string? EventHubName { get; set; }

    [CliFlag("--force-string")]
    public bool? ForceString { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--remove")]
    public string? Remove { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--set")]
    public string? Set { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--tables")]
    public string? Tables { get; set; }

    [CliOption("--workspace-name")]
    public string? WorkspaceName { get; set; }
}