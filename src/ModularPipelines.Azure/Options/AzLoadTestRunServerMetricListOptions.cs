using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("load", "test-run", "server-metric", "list")]
public record AzLoadTestRunServerMetricListOptions(
[property: CliOption("--load-test-resource")] string LoadTestResource,
[property: CliOption("--test-run-id")] string TestRunId
) : AzOptions
{
    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }
}