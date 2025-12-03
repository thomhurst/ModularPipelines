using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("monitor", "autoscale", "show-predictive-metric")]
public record AzMonitorAutoscaleShowPredictiveMetricOptions(
[property: CliOption("--aggregation")] string Aggregation,
[property: CliOption("--interval")] int Interval,
[property: CliOption("--metric-name")] string MetricName,
[property: CliOption("--metric-namespace")] string MetricNamespace,
[property: CliOption("--timespan")] string Timespan
) : AzOptions
{
    [CliOption("--autoscale-setting-name")]
    public string? AutoscaleSettingName { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}