using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("monitor", "app-insights", "query")]
public record AzMonitorAppInsightsQueryOptions(
[property: CliOption("--analytics-query")] string AnalyticsQuery
) : AzOptions
{
    [CliOption("--apps")]
    public string? Apps { get; set; }

    [CliOption("--end-time")]
    public string? EndTime { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--offset")]
    public string? Offset { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--start-time")]
    public string? StartTime { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}