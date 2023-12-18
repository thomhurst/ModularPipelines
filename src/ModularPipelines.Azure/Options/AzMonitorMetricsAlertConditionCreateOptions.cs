using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("monitor", "metrics", "alert", "condition", "create")]
public record AzMonitorMetricsAlertConditionCreateOptions(
[property: CommandSwitch("--aggregation")] string Aggregation,
[property: CommandSwitch("--metric")] string Metric,
[property: CommandSwitch("--op")] string Op,
[property: CommandSwitch("--type")] string Type
) : AzOptions
{
    [CommandSwitch("--dimension")]
    public string? Dimension { get; set; }

    [CommandSwitch("--namespace")]
    public string? Namespace { get; set; }

    [CommandSwitch("--num-periods")]
    public string? NumPeriods { get; set; }

    [CommandSwitch("--num-violations")]
    public string? NumViolations { get; set; }

    [CommandSwitch("--sensitivity")]
    public string? Sensitivity { get; set; }

    [CommandSwitch("--since")]
    public string? Since { get; set; }

    [BooleanCommandSwitch("--skip-metric-validation")]
    public bool? SkipMetricValidation { get; set; }

    [CommandSwitch("--threshold")]
    public string? Threshold { get; set; }
}

