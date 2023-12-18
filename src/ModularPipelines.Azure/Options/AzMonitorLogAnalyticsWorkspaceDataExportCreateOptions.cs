using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("monitor", "log-analytics", "workspace", "data-export", "create")]
public record AzMonitorLogAnalyticsWorkspaceDataExportCreateOptions(
[property: CommandSwitch("--data-export-name")] string DataExportName,
[property: CommandSwitch("--destination")] string Destination,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--tables")] string Tables,
[property: CommandSwitch("--workspace-name")] string WorkspaceName
) : AzOptions
{
    [BooleanCommandSwitch("--enable")]
    public bool? Enable { get; set; }

    [CommandSwitch("--event-hub-name")]
    public string? EventHubName { get; set; }
}

