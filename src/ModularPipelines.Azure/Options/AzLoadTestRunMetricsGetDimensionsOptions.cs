using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("load", "test-run", "metrics", "get-dimensions")]
public record AzLoadTestRunMetricsGetDimensionsOptions(
[property: CliOption("--load-test-resource")] string LoadTestResource,
[property: CliOption("--metric-definition-name")] string MetricDefinitionName,
[property: CliOption("--metric-dimension")] string MetricDimension,
[property: CliOption("--metric-namespace")] string MetricNamespace,
[property: CliOption("--test-run-id")] string TestRunId
) : AzOptions
{
    [CliOption("--end-time")]
    public string? EndTime { get; set; }

    [CliOption("--interval")]
    public int? Interval { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--start-time")]
    public string? StartTime { get; set; }
}