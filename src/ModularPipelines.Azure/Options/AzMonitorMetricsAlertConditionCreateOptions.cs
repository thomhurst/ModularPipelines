using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("monitor", "metrics", "alert", "condition", "create")]
public record AzMonitorMetricsAlertConditionCreateOptions(
[property: CliOption("--aggregation")] string Aggregation,
[property: CliOption("--metric")] string Metric,
[property: CliOption("--op")] string Op,
[property: CliOption("--type")] string Type
) : AzOptions
{
    [CliOption("--dimension")]
    public string? Dimension { get; set; }

    [CliOption("--namespace")]
    public string? Namespace { get; set; }

    [CliOption("--num-periods")]
    public string? NumPeriods { get; set; }

    [CliOption("--num-violations")]
    public string? NumViolations { get; set; }

    [CliOption("--sensitivity")]
    public string? Sensitivity { get; set; }

    [CliOption("--since")]
    public string? Since { get; set; }

    [CliFlag("--skip-metric-validation")]
    public bool? SkipMetricValidation { get; set; }

    [CliOption("--threshold")]
    public string? Threshold { get; set; }
}