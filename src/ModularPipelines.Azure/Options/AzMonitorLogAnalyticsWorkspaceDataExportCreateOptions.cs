using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("monitor", "log-analytics", "workspace", "data-export", "create")]
public record AzMonitorLogAnalyticsWorkspaceDataExportCreateOptions(
[property: CliOption("--data-export-name")] string DataExportName,
[property: CliOption("--destination")] string Destination,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--tables")] string Tables,
[property: CliOption("--workspace-name")] string WorkspaceName
) : AzOptions
{
    [CliFlag("--enable")]
    public bool? Enable { get; set; }

    [CliOption("--event-hub-name")]
    public string? EventHubName { get; set; }
}