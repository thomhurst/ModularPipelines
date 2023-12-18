using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("load", "test", "server-metric", "remove")]
public record AzLoadTestServerMetricRemoveOptions(
[property: CommandSwitch("--load-test-resource")] string LoadTestResource,
[property: CommandSwitch("--metric-id")] string MetricId,
[property: CommandSwitch("--test-id")] string TestId
) : AzOptions
{
    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [BooleanCommandSwitch("--yes")]
    public bool? Yes { get; set; }
}