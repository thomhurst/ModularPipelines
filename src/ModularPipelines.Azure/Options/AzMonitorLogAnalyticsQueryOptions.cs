using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("monitor", "log-analytics", "query")]
public record AzMonitorLogAnalyticsQueryOptions(
[property: CliOption("--analytics-query")] string AnalyticsQuery,
[property: CliOption("--workspace")] string Workspace
) : AzOptions
{
    [CliOption("--timespan")]
    public string? Timespan { get; set; }

    [CliOption("--workspaces")]
    public string? Workspaces { get; set; }
}