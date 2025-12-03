using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("monitor", "app-insights", "metrics", "show")]
public record AzMonitorAppInsightsMetricsShowOptions(
[property: CliOption("--metrics")] string Metrics
) : AzOptions
{
    [CliOption("--aggregation")]
    public string? Aggregation { get; set; }

    [CliOption("--app")]
    public string? App { get; set; }

    [CliOption("--end-time")]
    public string? EndTime { get; set; }

    [CliOption("--filter")]
    public string? Filter { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--interval")]
    public int? Interval { get; set; }

    [CliOption("--offset")]
    public string? Offset { get; set; }

    [CliOption("--orderby")]
    public string? Orderby { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--segment")]
    public string? Segment { get; set; }

    [CliOption("--start-time")]
    public string? StartTime { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--top")]
    public string? Top { get; set; }
}