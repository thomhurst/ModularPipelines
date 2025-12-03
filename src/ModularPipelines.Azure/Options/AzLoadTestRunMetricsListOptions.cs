using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("load", "test-run", "metrics", "list")]
public record AzLoadTestRunMetricsListOptions(
[property: CliOption("--load-test-resource")] string LoadTestResource,
[property: CliOption("--metric-namespace")] string MetricNamespace,
[property: CliOption("--test-run-id")] string TestRunId
) : AzOptions
{
    [CliOption("--aggregation")]
    public string? Aggregation { get; set; }

    [CliOption("--dimension-filters")]
    public string? DimensionFilters { get; set; }

    [CliOption("--end-time")]
    public string? EndTime { get; set; }

    [CliOption("--interval")]
    public int? Interval { get; set; }

    [CliOption("--metric-definition-name")]
    public string? MetricDefinitionName { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--start-time")]
    public string? StartTime { get; set; }
}