using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("load", "test", "server-metric", "add")]
public record AzLoadTestServerMetricAddOptions(
[property: CliOption("--aggregation")] string Aggregation,
[property: CliOption("--app-component-id")] string AppComponentId,
[property: CliOption("--app-component-type")] string AppComponentType,
[property: CliOption("--load-test-resource")] string LoadTestResource,
[property: CliOption("--metric-id")] string MetricId,
[property: CliOption("--metric-name")] string MetricName,
[property: CliOption("--metric-namespace")] string MetricNamespace,
[property: CliOption("--test-id")] string TestId
) : AzOptions
{
    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }
}