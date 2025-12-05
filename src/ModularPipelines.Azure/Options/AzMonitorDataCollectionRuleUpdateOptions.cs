using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("monitor", "data-collection", "rule", "update")]
public record AzMonitorDataCollectionRuleUpdateOptions : AzOptions
{
    [CliOption("--data-flows")]
    public string? DataFlows { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--extensions")]
    public string? Extensions { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--log-analytics")]
    public string? LogAnalytics { get; set; }

    [CliOption("--monitor-metrics")]
    public string? MonitorMetrics { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--performance-counters")]
    public int? PerformanceCounters { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--syslog")]
    public string? Syslog { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliOption("--windows-event-logs")]
    public string? WindowsEventLogs { get; set; }
}