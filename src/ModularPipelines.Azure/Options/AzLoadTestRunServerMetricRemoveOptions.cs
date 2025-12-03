using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("load", "test-run", "server-metric", "remove")]
public record AzLoadTestRunServerMetricRemoveOptions(
[property: CliOption("--load-test-resource")] string LoadTestResource,
[property: CliOption("--metric-id")] string MetricId,
[property: CliOption("--test-run-id")] string TestRunId
) : AzOptions
{
    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliFlag("--yes")]
    public bool? Yes { get; set; }
}