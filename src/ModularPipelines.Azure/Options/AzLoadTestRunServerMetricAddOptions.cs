using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("load", "test-run", "server-metric", "add")]
public record AzLoadTestRunServerMetricAddOptions(
[property: CliOption("--aggregation")] string Aggregation,
[property: CliOption("--app-component-id")] string AppComponentId,
[property: CliOption("--app-component-type")] string AppComponentType,
[property: CliOption("--load-test-resource")] string LoadTestResource,
[property: CliOption("--metric-id")] string MetricId,
[property: CliOption("--metric-name")] string MetricName,
[property: CliOption("--metric-namespace")] string MetricNamespace,
[property: CliOption("--test-run-id")] string TestRunId
) : AzOptions
{
    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }
}