using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("monitor", "data-collection", "rule", "update")]
public record AzMonitorDataCollectionRuleUpdateOptions : AzOptions
{
    [CommandSwitch("--data-flows")]
    public string? DataFlows { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--extensions")]
    public string? Extensions { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--log-analytics")]
    public string? LogAnalytics { get; set; }

    [CommandSwitch("--monitor-metrics")]
    public string? MonitorMetrics { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--performance-counters")]
    public int? PerformanceCounters { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }

    [CommandSwitch("--syslog")]
    public string? Syslog { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }

    [CommandSwitch("--windows-event-logs")]
    public string? WindowsEventLogs { get; set; }
}