using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("load", "test-run", "server-metric", "add")]
public record AzLoadTestRunServerMetricAddOptions(
[property: CommandSwitch("--aggregation")] string Aggregation,
[property: CommandSwitch("--app-component-id")] string AppComponentId,
[property: CommandSwitch("--app-component-type")] string AppComponentType,
[property: CommandSwitch("--load-test-resource")] string LoadTestResource,
[property: CommandSwitch("--metric-id")] string MetricId,
[property: CommandSwitch("--metric-name")] string MetricName,
[property: CommandSwitch("--metric-namespace")] string MetricNamespace,
[property: CommandSwitch("--test-run-id")] string TestRunId
) : AzOptions
{
    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }
}