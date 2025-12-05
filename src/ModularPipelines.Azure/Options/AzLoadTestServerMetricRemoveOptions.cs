using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("load", "test", "server-metric", "remove")]
public record AzLoadTestServerMetricRemoveOptions(
[property: CliOption("--load-test-resource")] string LoadTestResource,
[property: CliOption("--metric-id")] string MetricId,
[property: CliOption("--test-id")] string TestId
) : AzOptions
{
    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliFlag("--yes")]
    public bool? Yes { get; set; }
}