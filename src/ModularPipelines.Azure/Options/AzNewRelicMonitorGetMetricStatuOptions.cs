using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("new-relic", "monitor", "get-metric-statu")]
public record AzNewRelicMonitorGetMetricStatuOptions(
[property: CliOption("--user-email")] string UserEmail
) : AzOptions
{
    [CliOption("--azure-resource-ids")]
    public string? AzureResourceIds { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--monitor-name")]
    public string? MonitorName { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}