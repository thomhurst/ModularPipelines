using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("monitor", "log-analytics", "query")]
public record AzMonitorLogAnalyticsQueryOptions(
[property: CommandSwitch("--analytics-query")] string AnalyticsQuery,
[property: CommandSwitch("--workspace")] string Workspace
) : AzOptions
{
    [CommandSwitch("--timespan")]
    public string? Timespan { get; set; }

    [CommandSwitch("--workspaces")]
    public string? Workspaces { get; set; }
}

