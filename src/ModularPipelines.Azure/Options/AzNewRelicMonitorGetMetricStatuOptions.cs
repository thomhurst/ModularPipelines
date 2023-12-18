using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("new-relic", "monitor", "get-metric-statu")]
public record AzNewRelicMonitorGetMetricStatuOptions(
[property: CommandSwitch("--user-email")] string UserEmail
) : AzOptions
{
    [CommandSwitch("--azure-resource-ids")]
    public string? AzureResourceIds { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--monitor-name")]
    public string? MonitorName { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }
}