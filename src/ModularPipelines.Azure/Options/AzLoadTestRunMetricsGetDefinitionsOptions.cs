using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("load", "test-run", "metrics", "get-definitions")]
public record AzLoadTestRunMetricsGetDefinitionsOptions(
[property: CliOption("--load-test-resource")] string LoadTestResource,
[property: CliOption("--metric-namespace")] string MetricNamespace,
[property: CliOption("--test-run-id")] string TestRunId
) : AzOptions
{
    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }
}