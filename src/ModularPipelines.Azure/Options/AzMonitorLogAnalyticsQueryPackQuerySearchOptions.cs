using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("monitor", "log-analytics", "query-pack", "query", "search")]
public record AzMonitorLogAnalyticsQueryPackQuerySearchOptions : AzOptions
{
    [CommandSwitch("--categories")]
    public string? Categories { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [BooleanCommandSwitch("--include-body")]
    public bool? IncludeBody { get; set; }

    [CommandSwitch("--query-pack-name")]
    public string? QueryPackName { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--resource-types")]
    public string? ResourceTypes { get; set; }

    [CommandSwitch("--solutions")]
    public string? Solutions { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }
}