using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("monitor", "app-insights", "query")]
public record AzMonitorAppInsightsQueryOptions(
[property: CommandSwitch("--analytics-query")] string AnalyticsQuery
) : AzOptions
{
    [CommandSwitch("--apps")]
    public string? Apps { get; set; }

    [CommandSwitch("--end-time")]
    public string? EndTime { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--offset")]
    public string? Offset { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--start-time")]
    public string? StartTime { get; set; }

    [CommandSwitch("--subscription")]
    public new string? Subscription { get; set; }
}